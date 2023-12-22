using System;
using UnityEditor.IMGUI.Controls;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal static class PaletteEditorTreeViewExtensions
    {
        public static IObservable<PaletteEditorTreeViewEntryItem<T>> OnItemAddedAsObservable<T>(
            this PaletteEditorTreeView<T> self
        )
        {
            return new AnonymousObservable<PaletteEditorTreeViewEntryItem<T>>(observer =>
            {
                void OnNext(TreeViewItem item)
                {
                    try
                    {
                        observer.OnNext((PaletteEditorTreeViewEntryItem<T>)item);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }
                }

                self.OnItemAdded += OnNext;
                return new Disposable(() => self.OnItemAdded -= OnNext);
            });
        }

        public static IObservable<PaletteEditorTreeViewEntryItem<T>> OnItemRemovedAsObservable<T>(
            this PaletteEditorTreeView<T> self
        )
        {
            return new AnonymousObservable<PaletteEditorTreeViewEntryItem<T>>(observer =>
            {
                void OnNext(TreeViewItem item)
                {
                    try
                    {
                        observer.OnNext((PaletteEditorTreeViewEntryItem<T>)item);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }
                }

                self.OnItemRemoved += OnNext;
                return new Disposable(() => self.OnItemRemoved -= OnNext);
            });
        }
    }
}
