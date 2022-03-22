using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using uPalette.Editor.Foundation.EasyTreeView;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Core.PaletteEditor
{
    internal static class PaletteEditorTreeViewExtensions
    {
        public static IObservable<PaletteEditorTreeViewItem<T>> OnItemAddedAsObservable<T>(this PaletteEditorTreeView<T> self)
        {
            return new AnonymousObservable<PaletteEditorTreeViewItem<T>>(observer =>
            {
                void OnNext(TreeViewItem item)
                {
                    try
                    {
                        observer.OnNext((PaletteEditorTreeViewItem<T>)item);
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

        public static IObservable<PaletteEditorTreeViewItem<T>> OnItemRemovedAsObservable<T>(this PaletteEditorTreeView<T> self)
        {
            return new AnonymousObservable<PaletteEditorTreeViewItem<T>>(observer =>
            {
                void OnNext(TreeViewItem item)
                {
                    try
                    {
                        observer.OnNext((PaletteEditorTreeViewItem<T>)item);
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
