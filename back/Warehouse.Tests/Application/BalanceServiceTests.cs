using Warehouse.Application.Services;
using Warehouse.Domain.Models;
using Warehouse.Tests.TestInfrastructure;

namespace Warehouse.Tests.Application;

public class BalanceServiceTests
{

    public static IEnumerable<object[]> SimpleRemove()
    {
        yield return new object[] { 
            new List<IncomeItemEntity> {
                new IncomeItemEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 2 }
            },
            new List<BalanceEntity> {
                new BalanceEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 2 }
            }
        };
        yield return new object[] {
            new List<IncomeItemEntity> {
                new IncomeItemEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 3 }
            },
            new List<BalanceEntity> {
                new BalanceEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 3 }
            }
        };
        yield return new object[] {
            new List<IncomeItemEntity> {
                new IncomeItemEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 3 },
                new IncomeItemEntity { Id = 3, ResourceId = 2, UnitId = 2, Quantity = 4 }
            },
            new List<BalanceEntity> {
                new BalanceEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 3 },
                new BalanceEntity { Id = 3, ResourceId = 2, UnitId = 2, Quantity = 4 }
            }
        };
    }

    public static IEnumerable<object[]> SimpleEditCount()
    {
        yield return new object[] {
            new List<IncomeItemEntity> {
                new IncomeItemEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 1 },
                new IncomeItemEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 2 },
                new IncomeItemEntity { Id = 3, ResourceId = 2, UnitId = 2, Quantity = 3 }
            },
            new List<BalanceEntity> {
                new BalanceEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 1 },
                new BalanceEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 2 },
                new BalanceEntity { Id = 3, ResourceId = 2, UnitId = 2, Quantity = 3 }
            }
        };
        yield return new object[] {
            new List<IncomeItemEntity> {
                new IncomeItemEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 10 },
                new IncomeItemEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 20 },
                new IncomeItemEntity { Id = 3, ResourceId = 2, UnitId = 2, Quantity = 30 }
            },
            new List<BalanceEntity> {
                new BalanceEntity { Id = 1, ResourceId = 1, UnitId = 1, Quantity = 10 },
                new BalanceEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 20 },
                new BalanceEntity { Id = 3, ResourceId = 2, UnitId = 2, Quantity = 30 }
            }
        };
    }

    public static IEnumerable<object[]> EditCountAndType()
    {
        yield return new object[] {
            new List<IncomeItemEntity> {
                new IncomeItemEntity { Id = 1, ResourceId = 10, UnitId = 10, Quantity = 10 },
                new IncomeItemEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 20 },
                new IncomeItemEntity { Id = 3, ResourceId = 2, UnitId = 2, Quantity = 30 },
            },
            new List<BalanceEntity> {
                new BalanceEntity { Id = 4, ResourceId = 10, UnitId = 10, Quantity = 10 },
                new BalanceEntity { Id = 2, ResourceId = 2, UnitId = 1, Quantity = 20 },
                new BalanceEntity { Id = 3, ResourceId = 2, UnitId = 2, Quantity = 30 },
            }
        };
    }

    [Theory]
    //[MemberData(nameof(SimpleRemove))]
    //[MemberData(nameof(SimpleEditCount))]
    [MemberData(nameof(EditCountAndType))]
    public async Task TestCalculateAndApplyDifference(List<IncomeItemEntity> nowItems, List<BalanceEntity> result)
    {
        //Init oldState
        List<IncomeItemEntity> oldItems = new List<IncomeItemEntity> { 
            new IncomeItemEntity {Id = 1, ResourceId = 1, UnitId = 1, Quantity = 2 },
            new IncomeItemEntity {Id = 2, ResourceId = 2, UnitId = 1, Quantity = 3 },
            new IncomeItemEntity {Id = 3, ResourceId = 2, UnitId = 2, Quantity = 4 }
        };

        var balanceRepository = new TestBalanceRepository();
        //Init start balance
        foreach (var item in oldItems)
        {
            await balanceRepository.EditItem(
                new BalanceEntity { 
                    Id = item.Id, ResourceId = item.ResourceId, 
                    UnitId = item.UnitId, Quantity = item.Quantity
                });
        }

        var balanceService = new BalanceService(balanceRepository);

        await balanceService.CalculateAndApplyDifference(oldItems, nowItems);

        var allItems = await balanceRepository.GetAll(null);

        Assert.Equal(result.Count, allItems.Item1.Count);

        for(int i = 0; i < result.Count; i++)
        {
            var itemL = result[i];
            var itemR = allItems.Item1[i];

            Assert.Equal(itemL.Id, itemR.Id);
            Assert.Equal(itemL.ResourceId, itemR.ResourceId);
            Assert.Equal(itemL.UnitId, itemR.UnitId);
            Assert.Equal(itemL.Quantity, itemR.Quantity);
        }
    }
}
