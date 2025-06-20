/* using System;
using System.IO;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Factories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AbsanteeContext>
    {
        public AbsanteeContext CreateDbContext(string[] args)
        {
            // ---- LINHA DE DIAGNÓSTICO ----
            Console.WriteLine("--> DesignTimeDbContextFactory foi chamada. A tentar ler a configuração...");

            // Tenta encontrar o caminho para o projeto WebApi de forma mais robusta
            var webapiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "../WebApi");

            // Verifica se o caminho existe para ajudar no debug
            if (!Directory.Exists(webapiProjectPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"--> ERRO: O diretório do projeto WebApi não foi encontrado em: {Path.GetFullPath(webapiProjectPath)}");
                Console.ResetColor();
                throw new DirectoryNotFoundException("Não foi possível encontrar o diretório do projeto de startup (WebApi). Verifique o caminho em DesignTimeDbContextFactory.");
            }

            Console.WriteLine($"--> Usando o caminho base para configuração: {Path.GetFullPath(webapiProjectPath)}");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(webapiProjectPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AbsanteeContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A ConnectionString 'DefaultConnection' não foi encontrada no appsettings.json.");
            }

            Console.WriteLine("--> ConnectionString encontrada. A configurar o DbContext...");

            optionsBuilder.UseNpgsql(connectionString);

            Console.WriteLine("--> DbContext criado com sucesso.");

            return new AbsanteeContext(optionsBuilder.Options);
        }
    }
} */