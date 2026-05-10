namespace SortedTunes.Mediator.Contracts;

public interface IBaseRequest;

public interface IRequest : IBaseRequest;

public interface IRequest<out TResponse> : IBaseRequest;
