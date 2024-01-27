using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
             _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.POST,
                Data = couponDTO,
                URL = StaticDetails.CouponAPIBase + "/api/coupon/"
            });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.DELETE,
                URL = StaticDetails.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.GET,
                URL = StaticDetails.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> GetCouponByCodeAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.GET,
                URL = StaticDetails.CouponAPIBase + "/api/coupon/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDTO?> GetCouponByIDAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.GET,
                URL = StaticDetails.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = StaticDetails.APIType.PUT,
                Data = couponDTO,
                URL = StaticDetails.CouponAPIBase + "/api/coupon/"
            });
        }
    }
}
