using System;
using UnityEditor.IMGUI.Controls;
using uPalette.Runtime.Foundation.TinyRx;
#if UNITY_6000_2_OR_NEWER
using TreeViewItem = UnityEditor.IMGUI.Controls.TreeViewItem<int>;
#endif

namespace uPalette.Editor.Core.ThemeEditor
{
    internal static class ThemeEditorTreeViewExtensions
    {
        public static IObservable<ThemeEditorTreeViewItem> OnItemAddedAsObservable(this ThemeEditorTreeView self)
        {
            return new AnonymousObservable<ThemeEditorTreeViewItem>(observer =>
            {
                void OnNext(TreeViewItem item)
                {
                    try
                    {
                        observer.OnNext((ThemeEditorTreeViewItem)item);
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

        public static IObservable<ThemeEditorTreeViewItem> OnItemRemovedAsObservable(this ThemeEditorTreeView self)
        {
            return new AnonymousObservable<ThemeEditorTreeViewItem>(observer =>
            {
                void OnNext(TreeViewItem item)
                {
                    try
                    {
                        observer.OnNext((ThemeEditorTreeViewItem)item);
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
