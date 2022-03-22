using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using uPalette.Runtime.Foundation.TinyRx;

namespace uPalette.Editor.Foundation.TinyRx
{
    internal static class EditorObservableImplExtensions
    {
        public static System.IObservable<T> EditorBatchFrame<T>(this System.IObservable<T> self, int skipFrameCount = 0)
        {
            return new AnonymousObservable<T>(observer =>
            {
                var lastFrame = 0;
                var skippedFrameCount = 0;
                var values = new List<T>();

                void OnUpdate()
                {
                    if (lastFrame == Time.frameCount) return;

                    if (skippedFrameCount >= skipFrameCount)
                    {
                        if (values.Count >= 1)
                        {
                            var lastValue = values[values.Count - 1];
                            observer.OnNext(lastValue);
                            values.Clear();
                        }

                        skippedFrameCount = 0;
                    }
                    else
                    {
                        skippedFrameCount++;
                    }

                    lastFrame = Time.frameCount;
                }

                EditorApplication.update += OnUpdate;

                var disposable = self.Subscribe(x => { values.Add(x); }, x =>
                {
                    EditorApplication.update -= OnUpdate;
                    observer.OnError(x);
                }, () =>
                {
                    EditorApplication.update -= OnUpdate;
                    observer.OnCompleted();
                });

                var disposables = new CompositeDisposable();
                disposables.Add(new AnonymousDisposable(() => { EditorApplication.update -= OnUpdate; }));
                disposables.Add(disposable);
                return disposables;
            });
        }
    }
}
