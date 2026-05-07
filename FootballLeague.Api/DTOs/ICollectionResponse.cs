namespace FootballLeague.Api.DTOs;

public interface ICollectionResponse<T>
{
    List<T> Items { get; init; }
}
