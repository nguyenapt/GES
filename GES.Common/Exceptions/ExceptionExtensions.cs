using GES.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Common.Exceptions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// The method will invoke the action and try-catch the error
        /// </summary>
        /// <typeparam name="TWrapperException">Exception can be re-throw</typeparam>
        /// <typeparam name="TCatchedException"></typeparam>
        /// <param name="this"></param>
        /// <param name="logger"></param>
        /// <param name="action"></param>
        public static void Execute<TCatchedException, TWrapperException>(this object @this, IGesLogger logger, Action action)
            where TCatchedException : Exception
            where TWrapperException : Exception
        {
            Guard.AgainstNullArgument(nameof(action), action);
            Guard.AgainstNullArgumentProperty(nameof(action), "Method", action.Method);

            try
            {
                action();
            }
            catch (TCatchedException ex)
            {
                // Write log if need
                logger?.Error(ex, $"Exception when invoke action {action.Method.Name} in {@this.GetType().Name}.");

                // Re-throw exception
                throw (TWrapperException)Activator.CreateInstance(typeof(TWrapperException), $"Error when execute function {action.Method.Name}", ex);
            }
        }

        /// <summary>
        /// The method will invoke the action and try-catch the error
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TCatchedException"></typeparam>
        /// <typeparam name="TWrapperException"></typeparam>
        /// <param name="this"></param>
        /// <param name="logger"></param>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException">Throw when the function is null</exception>
        /// <returns></returns>
        public static TResult Execute<TResult, TCatchedException, TWrapperException>(this object @this, IGesLogger logger, Func<TResult> func)
            where TCatchedException : Exception
            where TWrapperException : Exception
        {
            Guard.AgainstNullArgument(nameof(func), func);
            Guard.AgainstNullArgumentProperty(nameof(func), "Method", func.Method);

            try
            {
                return func();
            }
            catch(TCatchedException ex)
            {
                // Write log if need
                logger?.Error(ex, $"Exception when invoke action {func.Method.Name} in {@this.GetType().Name}.");

                // Re-throw exception
                throw (TWrapperException)Activator.CreateInstance(typeof(TWrapperException), $"Error when execute function {func.Method.Name}", ex);
            }
        }
    }
}
