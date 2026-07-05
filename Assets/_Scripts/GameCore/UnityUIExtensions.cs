using System;
using System.Threading;
using R3;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

namespace Utils.Extensions
{
    public static class UnityUIExtension
    {
        public static IDisposable SubscribeToText(this Observable<string> source, TMP_Text text)
        {
            return source.Subscribe(text, static (x, t) => t.text = x);
        }

        public static IDisposable SubscribeToText<T>(this Observable<T> source, TMP_Text text)
        {
            return source.Subscribe(text, static (x, t) => t.text = x.ToString());
        }

        public static IDisposable SubscribeToText<T>(this Observable<T> source, TMP_Text text, Func<T, string> selector)
        {
            return source.Subscribe((text, selector), static (x, state) => state.text.text = state.selector(x));
        }

        public static IDisposable SubscribeToCommand(this Button button, ReactiveCommand<Unit> command)
        {
            return button.OnClickAsObservable().Subscribe(command, static (x, target) => target.Execute(x));
        }

        public static IDisposable SubscribeToCommand<T>(this Button button, ReactiveCommand<T> command,
            Func<T> selector)
        {
            return button.OnClickAsObservable()
                .Subscribe((command, selector),
                    static (x, state) => state.command.Execute(state.selector()));
        }

        public static IDisposable SubscribeToCommand<T>(this Observable<T> source, ReactiveCommand<T> command)
        {
            return source.Subscribe(command, static (x, target) => target.Execute(x));
        }

        public static IDisposable SubscribeToCommand<TIn, TOut>(this Observable<TIn> source,
            ReactiveCommand<TOut> command, Func<TIn, TOut> selector)
        {
            return source.Subscribe((command, selector),
                static (x, target) => target.command.Execute(target.selector(x)));
        }

        public static IDisposable SubscribeToReactiveProperty<T>(this Observable<T> source,
            ReactiveProperty<T> property)
        {
            return source.Subscribe(property, static (x, target) => target.Value = x);
        }

        public static IDisposable SubscribeToReactiveProperty<TIn, TOut>(this Observable<TIn> source,
            ReactiveProperty<TOut> property, Func<TIn, TOut> selector)
        {
            return source.Subscribe((property, selector),
                static (x, target) => target.property.Value = target.selector(x));
        }

        public static UniTask<T> ToNextValueUniTask<T>(this Observable<T> source, CancellationToken token = default)
        {
            return source.AsSystemObservable().ToUniTask(true, token);
        }
    }
}