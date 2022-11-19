using Neambc.Neamb.Feature.Rakuten.Repositories;
using Sitecore.DependencyInjection;
using Sitecore.Shell.Framework.Commands;

namespace Neambc.Neamb.Feature.Rakuten.Commands
{
    public class StoreImport : Command {
        /// <summary>Executes the command in the specified context.</summary>
        /// <param name="context">The context.</param>
        public override void Execute(CommandContext context) {
            var storeImportRepository =
                (IStoreImportRepository)ServiceLocator.ServiceProvider.GetService(typeof(IStoreImportRepository));
            storeImportRepository.ImportStores();
        }
    }
}