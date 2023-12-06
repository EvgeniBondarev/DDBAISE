using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

public class SessionLogger : ILogger
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionLogger(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
    {
        _loggerFactory = loggerFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return _loggerFactory.CreateLogger("SessionLogger").BeginScope(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _loggerFactory.CreateLogger("SessionLogger").IsEnabled(logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var logMessage = $"{DateTime.Now} - {formatter(state, exception)}";

        var logs = _httpContextAccessor.HttpContext.Session.GetString("Logs");
        logs = string.IsNullOrEmpty(logs) ? logMessage : $"{logs}\n{logMessage}";
        _httpContextAccessor.HttpContext.Session.SetString("Logs", logs);

        _loggerFactory.CreateLogger("SessionLogger").Log(logLevel, eventId, state, exception, formatter);
    }

    public List<string> GetSessionLogs()
    {
        var logs = _httpContextAccessor.HttpContext.Session.GetString("Logs");
        if (!string.IsNullOrEmpty(logs))
        {
            var logList = new List<string>(logs.Split('\n'));
            logList.Reverse();
            return logList;
        }

        return new List<string>();
    }

}
