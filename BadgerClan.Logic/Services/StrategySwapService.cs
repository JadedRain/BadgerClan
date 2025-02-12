using System.Runtime.Serialization;
using System.ServiceModel;

namespace BadgerClan.Logic.Services;

[DataContract]
public class StrategySwapRequest
{
    [DataMember(Order = 1)]
    public int StrategyId { get; set; }
}

[DataContract]
public class StrategySwapResponse
{
    [DataMember(Order = 1)]
    public string ConfirmationCode { get; set; }
    [DataMember(Order = 2)]
    public bool IsSuccess { get; set; }
    [DataMember(Order = 3)]
    public string Message { get; set; }
}


[ServiceContract]
public interface IStrategySwapService
{
    [OperationContract]
    public Task<StrategySwapResponse> StrategySwap(StrategySwapRequest request);
}


public class StrategySwapService : IStrategySwapService
{
    private MoveSetService _moveSetService;
    public StrategySwapService(MoveSetService moveSetService)
    {
        _moveSetService = moveSetService;
    }
    public Task<StrategySwapResponse> StrategySwap(StrategySwapRequest request)
    {
        _moveSetService.MoveSet = request.StrategyId;
        Console.WriteLine("Should be updated");
        return Task.FromResult(new StrategySwapResponse()
        {
            ConfirmationCode = Guid.NewGuid().ToString(),
            IsSuccess = true,
            Message = "Strategy Swapped"
        });
    }
}