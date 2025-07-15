
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.User;
using MobileFinance.Domain.Security.Tokens;

namespace MobileFinance.Application.UseCases.Login.External;
public class ExternalLoginUseCase : IExternalLoginUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public ExternalLoginUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        IUserWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<string> Execute(string name, string email)
    {
        var user = await _readOnlyRepository.GetByEmail(email);

        if(user is null)
        {
            user = new Domain.Entities.User
            {
                Name = name,
                Email = email,
                Password = "-"
            };

            await _writeOnlyRepository.Add(user);
            await _unitOfWork.Commit();
        }

        return _accessTokenGenerator.Generate(user.UserIdentifier);
    }
}
