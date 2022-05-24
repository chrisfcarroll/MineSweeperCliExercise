using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LogAssert;

public static class LogAssert
{
    /// <summary>If <paramref name="condition"/> evaluates true,
    /// then call <see cref="ILogger.LogWarning(string,object[]"/><c>(message, args)</c>.
    /// If <paramref name="@condition"/> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log">your Serilog logger</param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool WarnIf(this ILogger log, bool condition, string message, params object?[] args) 
        => If(log, LogLevel.Warning, condition, message, args);

    /// <summary>If <paramref name="condition"/> evaluates true,
    /// then call <see cref="ILogger.LogError(string,object[]"/><c>(message, args)</c>.
    /// If <paramref name="@condition"/> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log">your Serilog logger</param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool ErrorIf(this ILogger log, bool condition, string message, params object?[] args) 
        => If(log, LogLevel.Error, condition, message, args);
    

    /// <summary>If <paramref name="condition"/> evaluates true,
    /// then call <see cref="Information"/><c>(message, args)</c>.
    /// If <paramref name="@condition"/> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log">your Serilog logger</param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool InformationIf(this ILogger log, bool condition, string message, params object?[] args) 
        => If(log, LogLevel.Information, condition, message, args);
    
        
    /// <summary>If <paramref name="condition"/> evaluates true,
    /// then call <see cref="Debug"/><c>(message, args)</c>.
    /// If <paramref name="@condition"/> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log">your Serilog logger</param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool DebugIf(this ILogger log, bool condition, string message, params object?[] args) 
        => If(log, LogLevel.Debug, condition, message, args);
    
        
    /// <summary>If <paramref name="condition"/> evaluates true,
    /// then call <see cref="ILogger.LogWarning(string,object[]"/><c>(message, args)</c>.
    /// If <paramref name="@condition"/> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log">your Serilog logger</param>
    /// <param name="LogLevel">The <see cref="LogLevel"/> at which to log</param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="condition"/></returns>
    static bool If(ILogger log, LogLevel LogLevel, bool condition, string message, object?[] args)
    {
        try { if (condition) log.Log(LogLevel, message, args); } 
        catch (Exception e)
        {
            log.LogError(e, "({Message},{@args}) was true, but logging it threw an Exception", message, args);
        }

        return condition;
    }

    /// <summary>If <paramref name="condition"/> evaluates true,
    /// then call <see cref="ILogger.LogWarning(string,object[]"/><c>(message, args)</c>.
    /// If <paramref name="@condition"/> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log"></param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="delayedArgs">if <paramref name="condition"/> is true then this function is
    /// evaluated to generate args for the log. This allows logging of values that may be
    /// expensive to compute, or may be invalid when <c>condition</c> is false.</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool WarnIf<T>(this ILogger log, bool condition, string message, Func<T> delayedArgs) 
        => If(log, LogLevel.Warning, condition, message, delayedArgs);


    /// <summary>If <paramref name="condition"/> evaluates true,
    /// then call <see cref="ILogger.LogError(string,object[]"/><c>(message, args)</c>.
    /// If <paramref name="@condition"/> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log"></param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="delayedArgs">if <paramref name="condition"/> is true then this function is
    /// evaluated to generate args for the log. This allows logging of values that may be
    /// expensive to compute, or may be invalid when <c>condition</c> is false.</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool ErrorIf<T>(this ILogger log, bool condition, string message, Func<T> delayedArgs) 
        => If(log, LogLevel.Error, condition, message, delayedArgs);

    /// <summary>If <paramref name="condition"/> evaluates true,
    /// then call <see cref="Information"/><c>(message, args)</c>.
    /// If <paramref name="@condition"/> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log"></param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="delayedArgs">if <paramref name="condition"/> is true then this function is
    /// evaluated to generate args for the log. This allows logging of values that may be
    /// expensive to compute, or may be invalid when <c>condition</c> is false.</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool InformationIf<T>(this ILogger log, bool condition, string message, Func<T> delayedArgs) 
        => If(log, LogLevel.Information, condition, message, delayedArgs);


    /// <summary>If <paramref name="condition"/> evaluates true,
    /// then call <see cref="Debug"/><c>(message, args)</c>.
    /// If <paramref name="@condition"/> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log"></param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="delayedArgs">if <paramref name="condition"/> is true then this function is
    /// evaluated to generate args for the log. This allows logging of values that may be
    /// expensive to compute, or may be invalid when <c>condition</c> is false.</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool DebugIf<T>(this ILogger log, bool condition, string message, Func<T> delayedArgs) 
        => If(log, LogLevel.Debug, condition, message, delayedArgs);
    
    static bool If<T>(ILogger log, LogLevel LogLevel, bool condition, string message, Func<T> delayedArgs)
    {
        try { if (condition) log.Log(LogLevel, message, delayedArgs()); } 
        catch (Exception e)
        {
            log.LogError(e, "({Message},{@delayedArgs}) was true, but logging it threw an Exception", 
                message, delayedArgs);
        }
        return condition;
    }


    /// <summary>If <c>that(<paramref name="@this"/>)</c> evaluates true,
    /// then call <see cref="ILogger.LogWarning(string,object[]"/><c>(message, args)</c>.
    /// If <c>that(<paramref name="@this"/>)</c> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log"></param>
    /// <param name="@this">the object to test with <paramref name="that"/></param>
    /// <param name="that">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="that"/>(<paramref name="@this"/>).
    /// If evaluation throws an exception, then null is returned.</returns>
    public static bool? WarnIf<T>(this ILogger log, T @this, Func<T,bool> that, string message, params object[] args)
    {
        bool outcome;
        try { outcome= that(@this); if (outcome)log.LogWarning(message, args); }
        catch (Exception e)
        {
            log.LogError(e, "({Message},{@args}) threw when evaluating it.",message,args);
            return null;
        }
        return outcome;
    }

    /// <summary>If <c>that(<paramref name="@this"/>)</c> evaluates true,
    /// then call <see cref="ILogger.LogWarning(string,object[]"/><c>(message, args)</c>.
    /// If <c>that(<paramref name="@this"/>)</c> evaluates false, do nothing.
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log"></param>
    /// <param name="@this">the object to test with <paramref name="that"/></param>
    /// <param name="that">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="delayedArgs">if <c>that(<paramref name="@this"/>)</c> is true then this function is
    /// evaluated to generate args for the log. This allows logging of values that may be
    /// expensive to compute, or may be invalid when <c>that(<paramref name="@this"/>)</c> is false.</param>
    /// <returns><paramref name="condition"/></returns>
    /// <returns><paramref name="that"/>(<paramref name="@this"/>).
    /// If evaluation throws an exception, then null is returned.</returns>
    public static bool? WarnIf<T,Ta>(this ILogger log, T @this, Func<T,bool> that, string message, Func<Ta> delayedArgs)
    {
        bool outcome;
        try { outcome= that(@this); if (outcome)log.LogWarning(message, delayedArgs()); }
        catch (Exception e)
        {
            log.LogError(e, "({Message},{delayedArgs}) threw when evaluating it.",message,delayedArgs);
            return null;
        }
        return outcome;
    }
    
    
    /// <summary>If <paramref name="condition"/> is true, do nothing.
    /// If <paramref name="condition"/> is false, then
    /// call <see cref="ILogger.LogWarning(string,object[]"/><c>(message, args)</c>
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary> 
    /// <param name="log"></param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool WarnIfNot(this ILogger log, bool condition, string message, params object[] args)
    {
        return IfNot(log, LogLevel.Warning, condition, message, args);
    }

    /// <summary>If <paramref name="condition"/> is true, do nothing.
    /// If <paramref name="condition"/> is false, then
    /// call <see cref="ILogger.LogError(string,object[]"/><c>(message, args)</c>
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary> 
    /// <param name="log"></param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool ErrorIfNot(this ILogger log, bool condition, string message, params object[] args)
    {
        return IfNot(log, LogLevel.Error, condition, message, args);
    }

    /// <summary>If <paramref name="condition"/> is true, do nothing.
    /// If <paramref name="condition"/> is false, then
    /// call <see cref="Information"/><c>(message, args)</c>
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary> 
    /// <param name="log"></param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool InformationIfNot(this ILogger log, bool condition, string message, params object[] args)
    {
        return IfNot(log, LogLevel.Information, condition, message, args);
    }

    static bool IfNot(ILogger log, LogLevel LogLevel, bool condition, string message, object[] args)
    {
        try { if (!condition) log.Log(LogLevel,message, args); } 
        catch (Exception e)
        {
            log.LogError(e, "({Message},{@args}) was false, but logging it threw an Exception", message, args);
        }

        return condition;
    }

    /// <summary>If <c>that(<paramref name="@this"/>)</c> evaluates true, do nothing. 
    /// if <c>that(<paramref name="@this"/>)</c> evaluates false,
    /// then call <see cref="ILogger.LogWarning(string,object[]"/><c>(message, args)</c>
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log"></param>
    /// <param name="@this">the object to test with <paramref name="that"/></param>
    /// <param name="that">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="that"/>(<paramref name="@this"/>).
    /// If evaluation throws an exception, then null is returned.</returns>
    public static bool? WarnIfNot<T>(this ILogger log, T @this, Func<T,bool> that, string message, params object[] args)
    {
        bool outcome;
        try { outcome= that(@this); if (!outcome)log.LogWarning(message, args); }
        catch (Exception e)
        {
            log.LogError(e, "({Message},{@args}) threw when evaluating it.",message,args);
            return null;
        }
        return outcome;
    }


    /// <summary>AssertNotNull is an abbreviation for <see cref="ErrorIf"/>(<c><paramref name="it"/> is null</c>)
    /// If <paramref name="it"/> evaluates not null, do nothing. 
    /// If <paramref name="it"/> evaluates null,then call <see cref="ILogger.LogError(string,object[]"/><c>(message, args)</c>
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary>
    /// <param name="log"></param>
    /// <param name="it">the object under test</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="that"/></returns>
    [return:NotNull]
    public static T AssertNotNull<T>(this ILogger log, T? it, string message, params object[] args)
    {
        ErrorIf(log, it is null, message, args);
        return it!;
    }

    /// <summary>Log.Assert is a synonym for <see cref="ErrorIfNot"/>.
    /// If <paramref name="condition"/> is true, do nothing.
    /// If <paramref name="condition"/> is false then call <see cref="ILogger.LogError(string,object[]"/><c>(message, args)</c>
    /// If an exception is thrown during evaluation, then catch it and log that as an Error,
    /// and continue.</summary> 
    /// <param name="log"></param>
    /// <param name="condition">the condition</param>
    /// <param name="message">message format</param>
    /// <param name="args">args to message format</param>
    /// <returns><paramref name="condition"/></returns>
    public static bool Assert(this ILogger log, bool condition, string message, params object[] args) 
        => ErrorIfNot(log, condition, message, args);


    /// <summary>Log <paramref name="exception"/> and return it, but don't throw it.</summary>
    /// <param name="log"></param>
    /// <param name="exception"></param>
    /// <returns><paramref name="exception"/></returns>
    [return:NotNullIfNotNull("exception")]
    public static Exception? Exception(this ILogger log, Exception? exception, params object[] args)
    {
        if(exception is null){log.LogError("Tried to log a null exception {args}",args); return exception;}
        log.LogError(exception, exception.GetType().ToString(),args);
        return exception;
    }
    
    /// <summary>Log <paramref name="exception"/> and then throw it.
    /// Note that <see cref="System.Exception"/> is usually
    /// preferable, both because the compiler can understand it, and because the resulting stack
    /// trace is more to the point.</summary>
    /// <param name="log"></param>
    /// <param name="exception"></param>
    /// <exception><paramref name="exception"/> is thrown immediately after logging.</exception>
    /// <see cref="System.Exception"/>
    /// <remarks>Note that <see cref="System.Exception"/> is usually
    /// preferable, both because the compiler can understand it, and because the resulting stack
    /// trace is more to the point.</remarks>
    /// <returns>Never returns. The exception is thrown.</returns>
    public static Exception ExceptionThenThrow(this ILogger log, Exception exception, params object[] args)
    {
        exception ??= new Exception("Tried to log a null exception");
        log.LogError(exception, exception.GetType().ToString(), args);
        throw exception;
    }
    
    /// <summary>Log <paramref name="exception"/> and then <see cref="Environment.Exit"/>> the current Process</summary>
    /// <param name="log"></param>
    /// <param name="exception"></param>
    /// <param name="exitCode">The exitCode to return to the operating system or calling process</param>
    /// <returns>Nothing. The Process is halted.</returns>
    public static void ExceptionThenSystemExitCurrentProcessWithExitCode(this ILogger log, Exception exception, int exitCode, params object[] args)
    {
        if(exception is null){log.LogError("Tried to log a null exception {args}",args);}
        else { log.LogError(exception, exception.GetType().ToString(), args); }
        System.Environment.Exit(exitCode);
    }

    /// <summary>Create a new <see cref="ApplicationException"/> with <paramref name="message"/>,
    /// log it with <paramref name="args"/>, then return it. But don't throw it.</summary>
    /// <param name="log"></param>
    /// <param name="message"></param>
    /// <returns>The new <see cref="ApplicationException"/> with <paramref name="message"/>
    /// Returns null if there is no message and no arguments.
    /// </returns>
    public static ApplicationException? Exception(this ILogger log, string message,params object[] args)
    {
        if(string.IsNullOrWhiteSpace(message) && args.Length==0){log.LogError("Tried to log an empty exception"); return null;}
        var ex = new ApplicationException(message);
        log.LogError(ex, message??"{args}",args);
        return ex;
    }

    /// <summary>If <paramref name="that"/> is true do nothing.
    /// Otherwise log it as Error and throw the Exception generated by <paramref name="exception"/>
    /// If an exception is thrown during evaluation, then log that as an Error,
    /// and throw that instead.</summary>
    /// <param name="log"></param>
    /// <param name="that">the condition</param>
    /// <param name="exception">an exception to throw if <paramref name="that"/> is false.</param>
    /// <param name="args">will be passed to <see cref="ILogger.LogError"/> if <paramref name="that"/> is false</param>
    /// <exception cref="System.Exception"><paramref name="exception"/></exception>
    /// <returns><paramref name="that"/>.</returns>
    public static bool EnsureElseThrow(this ILogger log, bool that, Exception exception, params object[] args)
    {
        try { if (that) return that; log.LogError(exception,messageWasFalse,args); throw exception;}
        catch (Exception e) { log.LogError(e,messageThrew,args); throw;}
    }
    
    /// <summary>If <paramref name="that"/> is true do nothing.
    /// Otherwise log it as Error and throw the Exception generated by <paramref name="exception"/>
    /// If an exception is thrown during evaluation, then log that as an Error,
    /// and throw that instead.</summary>
    /// <param name="log"></param>
    /// <param name="that">the condition</param>
    /// <param name="exception">a function that will generate the exception</param>
    /// <param name="args">will be passed to <see cref="ILogger.LogError"/> as args,
    /// if <paramref name="that"/> is false</param>
    /// <exception cref="System.Exception"><paramref name="exception"/></exception>
    /// <returns><paramref name="that"/></returns>
    public static bool EnsureElseThrow(this ILogger log, bool that, Func<Exception> exception, params object[] args)
    {
        try { if (that) return that; var ex = exception();  log.LogError(ex,messageWasFalse,args); throw ex;}
        catch (Exception e) { log.LogError(e,messageThrew,args); throw;}
    }

    
    /// <summary>If <paramref name="that"/> is true do nothing.
    /// Otherwise log it as Error and throw the Exception generated by <paramref name="exception"/>
    /// If an exception is thrown during evaluation, then log that as an Error,
    /// and throw that instead.</summary>
    /// <param name="log"></param>
    /// <param name="that">the condition</param>
    /// <param name="exception">a function that will generate the exception</param>
    /// <param name="delayedArgs">will be evaluated only  if <paramref name="that"/> is false,
    /// then passed as args to <see cref="ILogger.LogError"/></param>
    /// <exception cref="System.Exception"><paramref name="exception"/></exception>
    /// <returns><paramref name="that"/></returns>
    public static bool EnsureElseThrow(this ILogger log, bool that, Func<Exception> exception, Func<object[]> delayedArgs)
    {
        try { 
            if (that) return that; 
            var ex = exception();  
            log.LogError(ex,messageWasFalse,delayedArgs()); 
            throw ex;
            
        }
        catch (Exception e) { log.LogError(e,messageThrew,"(Func<object[]> delayedArgs not evaluated)"); throw;}
    }


    const string messageThisThrew = "EnsureElseLogAndThrow({@this},) threw during evaluation.";
    const string messageThisWasFalse = "EnsureElseLogAndThrow({@this},) was false.";
    const string messageThrew = "EnsureElseLogAndThrow threw during evaluation. {args}";
    const string messageWasFalse = "EnsureElseLogAndThrow was false. {args}";
    static readonly string nl = Environment.NewLine;
    static readonly Exception FailedToEvaluateOrGetExceptionException 
        = new Exception("Failed to evaluate an assertion and failed to evaluate the exception to describe it");
}