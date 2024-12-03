﻿using Basket.API.Data;
using Discount.Grpc;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class StoreBasketHandler
        (
        IBasketRepository repository,
        DiscountProtoService.DiscountProtoServiceClient discountService
        )
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.Cart, cancellationToken);

            await repository.StoreBasket(command.Cart, cancellationToken);

            return new StoreBasketResult(command.Cart.UserName);
        }

        public async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountService.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName },cancellationToken:cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }
}
