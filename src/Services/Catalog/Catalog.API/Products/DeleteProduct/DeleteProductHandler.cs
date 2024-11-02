﻿
namespace Catalog.API.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid id):ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);
    internal class DeleteProductCommandHandler
        (IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductCommandHandler is called with {@Command}", command);
            
            session.Delete(command.id);

            await session.SaveChangesAsync();

            return new DeleteProductResult(true);
        }
    }
}