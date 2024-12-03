using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(
        DiscountContext dbContext, 
        ILogger<DiscountService> logger)
        :DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext
                            .Coupons
                            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if(coupon == null)
                new Coupons { Id=1,ProductName="No Product", Amount=0, Description="No Discount for Product"};

            logger.LogInformation("Discount Retrieved for product {productname} with Discount amount {amount}", coupon!.ProductName, coupon.Amount);

            var couponModel= coupon.Adapt<CouponModel>();
            return couponModel;            
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupons>();

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request Argument"));

            dbContext.Add(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Coupon created successfully for Product {productName}",coupon!.ProductName);

            var couponmodel = coupon.Adapt<CouponModel>();
            return couponmodel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupons>();

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request Argument"));

            dbContext.Update(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Coupon updated successfully for Product {productName}", coupon!.ProductName);

            var couponmodel = coupon.Adapt<CouponModel>();
            return couponmodel;
        }
        public override async Task<DeleteRequestResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = dbContext
                .Coupons
                .FirstOrDefaultAsync(x=>x.ProductName == request.ProductName);

            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found"));

            dbContext.Remove(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Coupon Deleted successfully. ProductName: {productName}",request.ProductName);

            return new DeleteRequestResponse { Success = true };
        }
    }
}
