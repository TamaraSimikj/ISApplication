using Domain.DTO;

namespace Service.Interface
{
    public interface ICardService
    {
        public ShoppingCardDTO GetCardInfo(string id);
        
        public bool DeleteProductFromCard(string userId,Guid? id);

        public bool OrderProductsAsync(string id);
    }
}