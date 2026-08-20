using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using uPalette.Editor.Core.PaletteEditor;
using uPalette.Editor.Core.Shared;
using uPalette.Runtime.Core;
using uPalette.Runtime.Core.Model;

namespace uPalette.Tests.Editor
{
    public sealed class PaletteEntryOrderingTest
    {
        [Test]
        public void GetOrderedEntries_FolderHierarchyIsComparedBeforeFullEntryPath()
        {
            var palette = new ColorPalette();

            // フラットなエントリを、同じ接頭辞を持つフォルダ内エントリより先に保存しておく。
            var flatEntry = palette.AddEntry();
            flatEntry.Name.Value = "A.Thing";
            var folderEntry = palette.AddEntry();
            folderEntry.Name.Value = "A/Z";

            // フォルダ表示と同じ階層順でエントリを取得する。
            var orderedEntryIds = PaletteEntryOrdering.GetOrderedEntries(palette, true, '/')
                .Select(entry => entry.Id)
                .ToArray();

            // ルートではフォルダ名のAがA.Thingより先になるため、フォルダ内エントリが先になる。
            Assert.That(orderedEntryIds, Is.EqualTo(new[] { folderEntry.Id, flatEntry.Id }));
        }

        [Test]
        public void SetFolderMode_WhenReturningToFlatMode_RestoresDragAndDropOrder()
        {
            var treeView = new ColorPaletteEditorTreeView(new TreeViewState());
            var alphaEntry = treeView.AddItem("alpha", "Alpha", new Dictionary<string, Color>());
            var betaEntry = treeView.AddItem("beta", "Beta", new Dictionary<string, Color>());

            // フラット表示でBetaをAlphaより前へドラッグした状態にする。
            treeView.SetFlatEntryIndex(betaEntry, 0, false);

            // フォルダ表示へ切り替えて名前順にした後、フラット表示へ戻す。
            treeView.SetFolderMode(true, false);
            treeView.SetFolderMode(false, false);
            var entryIds = treeView.RootItem.children
                .Cast<PaletteEditorTreeViewEntryItem<Color>>()
                .Select(entry => entry.EntryId)
                .ToArray();

            // フラット表示では、切り替え前のドラッグ&ドロップ順に復帰する。
            Assert.That(entryIds, Is.EqualTo(new[] { betaEntry.EntryId, alphaEntry.EntryId }));
        }

        [Test]
        public void SetFlatEntryIndex_InFolderMode_UpdatesOnlyTheSavedFlatOrder()
        {
            var treeView = new ColorPaletteEditorTreeView(new TreeViewState());
            var alphaEntry = treeView.AddItem("alpha", "Alpha", new Dictionary<string, Color>());
            var betaEntry = treeView.AddItem("beta", "Beta", new Dictionary<string, Color>());

            // フォルダ表示中にUndoまたはRedoがフラット順をBeta、Alphaへ更新した状態にする。
            treeView.SetFolderMode(true, false);
            treeView.SetFlatEntryIndex(betaEntry, 0, false);
            var folderModeEntryIds = GetEntryIds(treeView.RootItem).ToArray();

            // フォルダ表示は名前順のまま維持される。
            Assert.That(folderModeEntryIds, Is.EqualTo(new[] { alphaEntry.EntryId, betaEntry.EntryId }));

            // フラット表示へ戻す。
            treeView.SetFolderMode(false, false);
            var flatModeEntryIds = GetEntryIds(treeView.RootItem).ToArray();

            // UndoまたはRedoで更新したフラット順が表示へ反映される。
            Assert.That(flatModeEntryIds, Is.EqualTo(new[] { betaEntry.EntryId, alphaEntry.EntryId }));
        }

        [Test]
        public void EntryOrderRedo_InFolderMode_RestoresTheFlatOrderInTreeView()
        {
            var store = ScriptableObject.CreateInstance<PaletteStore>();
            var generateService = new GenerateNameEnumsFileService(store);
            var wasNameEnumsDirty = generateService.IsDirty;
            var editService = new EditPaletteStoreService(store, generateService);
            var view = new ColorPaletteEditorWindowContentsView();
            view.Setup();
            var palette = store.ColorPalette;
            var alphaEntry = palette.AddEntry();
            alphaEntry.Name.Value = "Alpha";
            var betaEntry = palette.AddEntry();
            betaEntry.Name.Value = "Beta";
            var controller = new PaletteEditorWindowContentsViewController<Color>(palette, editService, view);
            var presenter = new PaletteEditorWindowContentsViewPresenter<Color>(palette, view);

            try
            {
                // フラット表示でBetaを先頭へ移動し、その並べ替えを履歴へ登録する。
                var betaItem = view.TreeView.RootItem.children
                    .Cast<PaletteEditorTreeViewEntryItem<Color>>()
                    .Single(item => item.EntryId == betaEntry.Id);
                view.TreeView.SetFlatEntryIndex(betaItem, 0, true);

                // フォルダ表示へ切り替えた状態で、並べ替えをUndoしてからRedoする。
                view.TreeView.SetFolderMode(true, false);
                editService.Undo();
                editService.Redo();

                // フラット表示へ戻す。
                view.TreeView.SetFolderMode(false, false);
                var entryIds = GetEntryIds(view.TreeView.RootItem).ToArray();

                // モデルとTreeViewの両方が、Redo後のBeta、Alphaの順になっている。
                Assert.That(palette.GetEntryOrder(betaEntry.Id), Is.EqualTo(0));
                Assert.That(entryIds, Is.EqualTo(new[] { betaEntry.Id, alphaEntry.Id }));
            }
            finally
            {
                controller.Dispose();
                presenter.Dispose();
                view.Dispose();
                editService.ClearDirty();
                editService.Dispose();

                if (wasNameEnumsDirty)
                    generateService.MarkDirty();
                else
                    generateService.ClearDirty();

                Object.DestroyImmediate(store);
            }
        }

        [Test]
        public void FolderMode_TreeViewAndNameEnumsUseTheSameEntryOrder()
        {
            var palette = new ColorPalette();
            var flatEntry = palette.AddEntry();
            flatEntry.Name.Value = "A.Thing";
            var folderEntry = palette.AddEntry();
            folderEntry.Name.Value = "A/Z";
            var treeView = new ColorPaletteEditorTreeView(new TreeViewState());
            treeView.AddItem(flatEntry.Id, flatEntry.Name.Value, new Dictionary<string, Color>());
            treeView.AddItem(folderEntry.Id, folderEntry.Name.Value, new Dictionary<string, Color>());

            // TreeViewをフォルダ表示にし、Name Enums用データもフォルダ表示の設定で作成する。
            treeView.SetFolderMode(true, false);
            var treeViewEntryIds = GetEntryIds(treeView.RootItem).ToArray();
            var paletteData = GenerateNameEnumsFileService.CreatePaletteData(
                nameof(Color),
                palette,
                '/',
                true,
                true);
            var nameEnumsEntryIds = paletteData.EntryInfos.Select(entry => entry.id).ToArray();

            // Palette EditorとName Enumsで同じエントリ順になる。
            Assert.That(nameEnumsEntryIds, Is.EqualTo(treeViewEntryIds));
        }

        [TestCase(false, "Z")]
        [TestCase(true, "A_Z")]
        public void CreatePaletteData_FolderNameOptionDoesNotChangeEntryOrder(
            bool containsFolderName,
            string expectedFolderEntryName
        )
        {
            var palette = new ColorPalette();

            // フォルダ名を含める設定にかかわらず、階層順ではAフォルダが先になるエントリを用意する。
            var flatEntry = palette.AddEntry();
            flatEntry.Name.Value = "A.Thing";
            var folderEntry = palette.AddEntry();
            folderEntry.Name.Value = "A/Z";

            // フォルダ名の出力設定を指定して、Name Enums用データを作成する。
            var paletteData = GenerateNameEnumsFileService.CreatePaletteData(
                nameof(Color),
                palette,
                '/',
                containsFolderName,
                true);
            var entryNames = paletteData.EntryInfos.Select(entry => entry.name).ToArray();

            // enum名だけが設定に応じて変わり、エントリ順は変わらない。
            Assert.That(entryNames, Is.EqualTo(new[] { expectedFolderEntryName, "A.Thing" }));
        }

        [Test]
        public void CreatePaletteData_FlatModeUsesDragAndDropOrder()
        {
            var palette = new ColorPalette();
            var firstEntry = palette.AddEntry();
            firstEntry.Name.Value = "First";
            var secondEntry = palette.AddEntry();
            secondEntry.Name.Value = "Second";

            // Palette Editorのドラッグ&ドロップに相当する保存順として、Secondを先頭へ移動する。
            palette.SetEntryOrder(secondEntry.Id, 0);

            // フォルダ表示を使わない設定で、Name Enums用データを作成する。
            var paletteData = GenerateNameEnumsFileService.CreatePaletteData(
                nameof(Color),
                palette,
                '/',
                false,
                false);
            var entryIds = paletteData.EntryInfos.Select(entry => entry.id).ToArray();

            // Name Enumsには、Palette Editorで指定したドラッグ&ドロップ順が使われる。
            Assert.That(entryIds, Is.EqualTo(new[] { secondEntry.Id, firstEntry.Id }));
        }

        private static IEnumerable<string> GetEntryIds(TreeViewItem parent)
        {
            if (parent.children == null)
                yield break;

            foreach (var child in parent.children)
            {
                if (child is PaletteEditorTreeViewEntryItem<Color> entryItem)
                    yield return entryItem.EntryId;

                foreach (var entryId in GetEntryIds(child))
                    yield return entryId;
            }
        }
    }
}
