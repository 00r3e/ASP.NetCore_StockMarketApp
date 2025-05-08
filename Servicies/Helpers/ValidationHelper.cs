using System.ComponentModel.DataAnnotations;
using ServicesContracts.DTO;


namespace Servicies.Helpers
{
    public class ValidationHelper
    {
        internal static void ModelValidation(object model)
        {
            ValidationContext validationContext = new ValidationContext(model);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults);

            if (model is BuyOrderRequest buyOrderRequest)
            {
                if (buyOrderRequest.Quantity < 1 || buyOrderRequest.Quantity > 100000)
                {
                    isValid = false;
                }
                if (buyOrderRequest.Price < 1 || buyOrderRequest.Price > 100000)
                {
                    isValid = false;
                }
            }

            if (model is SellOrderRequest sellOrderRequest)
            {
                if (sellOrderRequest.Quantity < 1 || sellOrderRequest.Quantity > 100000)
                {
                    isValid = false;
                }
                if (sellOrderRequest.Price < 1 || sellOrderRequest.Price > 100000)
                {
                    isValid = false;
                }
            }

            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
