namespace Warehouse.Contracts.Api.Response;

public class ResponseDtoEmpty : ResponseDto<object?>
{
    public ResponseDtoEmpty() : base(null)
    {
    }
}
