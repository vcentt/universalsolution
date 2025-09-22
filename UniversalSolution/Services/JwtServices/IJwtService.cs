using UniversalSolution.Domain.Entities;

namespace UniversalSolution.Services.JwtServices;

public interface IJwtService
{
    string CreateToken(User user);
}