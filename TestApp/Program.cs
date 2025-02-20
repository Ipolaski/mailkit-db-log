// See https://aka.ms/new-console-template for more information
#region [usings]
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using TestApp.Infrastructure.EmailSender;
using TestApp.Infrastructure.Logger;
#endregion

const string _logName = "log.txt";
string pathToLog = Path.Combine(Directory.GetCurrentDirectory(), _logName);
#region [logger]
ILoggerFactory loggerFactory = new LoggerFactory();
loggerFactory.AddFile(pathToLog);
var logger = loggerFactory.CreateLogger("FileLogger");
#endregion

#region [configuration]
IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                                                       .SetBasePath(Directory.GetCurrentDirectory())
                                                       .AddJsonFile("appsettings.json", true, true);
IConfiguration _config = configurationBuilder.Build();
#endregion

logger.LogInformation($"Start app at {DateTime.Now}");

using ( TestApp.Infrastructure.AppContext appContext = new() )
{
    FormattableString sql;
    if ( appContext.UseSqlServer )
        sql = $@"select Id, Path 
                from Files F 
                where F.Id not in ( 
                    select Fl.Id
                    from Files Fl 
                    join Table2s T2 on Fl.Id = T2.FileId )";
    else
        sql = $@"select ""Id"", ""Path"" 
                from ""Files"" F 
                where F.""Id"" not in ( 
                    select Fl.""Id""
                    from ""Files"" Fl 
                    join ""Table2s"" T2 on Fl.""Id"" = T2.""FileId"" )";
        var valuesWithoutForeignKeys = appContext.Files.FromSql(sql).ToList();

    foreach ( var removedFile in valuesWithoutForeignKeys )
    {
        logger.LogInformation($"Delete file at {removedFile.Path}");
        File.Delete(removedFile.Path);
    }

    appContext.RemoveRange(valuesWithoutForeignKeys);
    appContext.SaveChanges();
}

var emailAdress = _config.GetSection("Email");

if ( emailAdress.Value != null )
{
    EmailService emailService = new();
    await emailService.SendEmailAsync(emailAdress.Value, "LogFile", pathToLog);
}

Console.ReadLine();