using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using uPalette.Editor.Foundation.EasyTreeView;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Foundation
{
    internal static class TreeViewBaseExtensions
    {
        public static IObservable<TreeViewItem> OnItemAddedAsObservable(this TreeViewBase self)
        {
            return new AnonymousObservable<TreeViewItem>(observer =>
            {
                void OnNext(TreeViewItem item)
                {
                    try
                    {
                        observer.OnNext(item);
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

        public static IObservable<TreeViewItem> OnItemRemovedAsObservable(this TreeViewBase self)
        {
            return new AnonymousObservable<TreeViewItem>(observer =>
            {
                void OnNext(TreeViewItem item)
                {
                    try
                    {
                        observer.OnNext(item);
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

        public static IObservable<Empty> OnItemClearedAsObservable(this TreeViewBase self)
        {
            return new AnonymousObservable<Empty>(observer =>
            {
                void OnNext()
                {
                    try
                    {
                        observer.OnNext(Empty.Default);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }
                }

                self.OnItemsCleared += OnNext;
                return new Disposable(() => self.OnItemsCleared -= OnNext);
            });
        }

        public static IObservable<IList<int>> OnSelectionChangedAsObservable(this TreeViewBase self)
        {
            return new AnonymousObservable<IList<int>>(observer =>
            {
                void OnNext(IList<int> ids)
                {
                    try
                    {
                        observer.OnNext(ids);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }
                }

                self.OnSelectionChanged += OnNext;
                return new Disposable(() => self.OnSelectionChanged -= OnNext);
            });
        }
    }
}
