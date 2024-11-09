﻿
namespace Catalog.API.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid id):ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);


    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Product Id is required");
        }
    }
    internal class DeleteProductCommandHandler
        (IDocumentSession session)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {         
            
            session.Delete(command.id);

            await session.SaveChangesAsync();

            return new DeleteProductResult(true);
        }
    }
}
