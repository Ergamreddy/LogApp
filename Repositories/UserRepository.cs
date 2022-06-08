using Dapper;
using main project.Utilities;
using main project.Models;
using Microsoft.AspNetCore.Authorization;

namespace main project.Repositories;

public interface IUserRepository
{
    Task<User> GetByEmail(string Email);

    Task<User> Create(User Item);

    Task<bool> Update(User Item);
    Task<User> GetUserById(int Id);


}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<User> Create(User Item)
    {
        var query = $@"INSERT INTO ""{TableNames.user}"" (full_name, email, password)
	VALUES (@FullName, @Email, @Password)
        RETURNING *";


        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<User>(query, Item);
            return res;
        }

        // public async Task<Users> GetByEmail(string Email)


        // {
        //     var query = $@"SELECT * FROM ""{TableNames.user}""
        // WHERE email = @Email";

        //     using (var con = NewConnection)

        //         return await con.QuerySingleOrDefaultAsync<Users>(query, new { Email });
        // }

        // public async Task<Users> GetByUserName(string UserName)
        // {
        //     var query = $@"SELECT * FROM ""{TableNames.user}""
        //     WHERE username = @UserName";

        //     using (var con = NewConnection)

        //         return await con.QuerySingleOrDefaultAsync<Users>(query, new { UserName });


        // }

        //  public Task<bool> Update(Users Item)


        // {
        //     throw new NotImplementedException();
        // }
    }

    public async Task<User> GetByEmail(string Email)
    {
        {
            var query = $@"SELECT * FROM ""{TableNames.user}""
        WHERE email = @Email";

            using (var con = NewConnection)

                return await con.QuerySingleOrDefaultAsync<User>(query, new { Email });
        }


    }

    public async Task<User> GetUserById(int Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.user}"" WHERE id = @Id";
        using (var con = NewConnection)

            return await con.QuerySingleOrDefaultAsync<User>(query, new { Id });

    }

    public async Task<bool> Update(User Item)
    {
        var query = $@"UPDATE ""{TableNames.user}"" SET full_name = @FullName
          WHERE id = @Id";

        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);
            return rowCount == 1;
        }
    }
}
