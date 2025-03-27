using Dotnet.Template.Data;
using Dotnet.Template.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Dotnet.Template.Integrations
{
    public class UserService(
        IConfiguration configuration,
        MySqlContext context
            ) : BaseService<User>(configuration)
    {
        private readonly MySqlContext _context = context;
        private readonly IConfiguration _configuration = configuration;
        private readonly RestClient _restClient = new("", configureSerialization: c => c.UseNewtonsoftJson());

        public override async Task<bool> UpdateAsync(object id, int attempts = 0)
        {
            var user = new UsersResponse();
            if (user == null)
            {
                LogError($"Não foi possivel localizar o registro para: {id}");
                return false;
            }

            var invitaItemsMapped = MapResponseToDatabase(user);
            return await UpdateAsync(invitaItemsMapped, attempts);
        }

        public override async Task UpdateAsync()
        {
            int offset = 1;
            int itemsFounded = 0;
            bool hasNextPage = true;

            do
            {
                var users = new RestRequest($"");
                users.AddHeader("Authorization", $"Bearer");
                var usersResponse = await _restClient.GetAsync<ApiResponse<IEnumerable<UsersResponse>>>(users);
                var responseItems = usersResponse.Data;

                if (responseItems == null) break;
                itemsFounded = responseItems.Count();

                LogInfo($"Processando lote {offset} | Itens: {itemsFounded}");

                var itemsMapped = responseItems
                    .Where(p => p.UpdateDate > UpdateStartAt)
                    .Select(MapResponseToDatabase);

                foreach (var item in itemsMapped)
                {
                    await UpdateAsync(item);
                }
                offset++;

            } while (hasNextPage);

            LogRegisterProccessError();
            LogRegisterAdded();
            LogRegisterUpdated();
        }

        protected override async Task<bool> UpdateAsync(User item, int attempts = 0)
        {
            var workingItem = await _context.Users.FirstOrDefaultAsync(p => p.Id == item.Id) ?? item;
            try
            {
                if (workingItem.Id == 0)
                {
                    LogInfo($"ADICIONANDO | {workingItem.Id}");
                    _context.Add(workingItem);
                    ItemsAdded++;
                }
                else if (MustBeUpdated(workingItem, item))
                {
                    LogInfo($"ATUALIZANDO | {workingItem.Id}");
                    UpdateAFromB(workingItem, item);
                    _context.Update(workingItem);
                    ItemsUpdated++;
                }
                if (_context.ChangeTracker.HasChanges())
                {
                    _context.SaveChanges();
                    _context.Entry(workingItem).State = EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                var actionState = _context.Entry(workingItem).State;
                _context.Entry(workingItem).State = EntityState.Detached;
                if (attempts == 0 && ex is DbUpdateException && ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    return await UpdateAsync(item, attempts + 1);
                }
                ItemsFailed++;
                LogError($"Erro processando o objeto: {actionState} - {item.Id}");
                LogError(ex.Message);
                LogError(ex.InnerException?.Message);
                return false;
            }
        }

        private static bool MustBeUpdated(User userA, User userB)
        {
            var commonDataUpToDate =
                userA.Name == userB.Name &&
                userA.Id == userB.Id &&
                userA.Email == userB.Email;

            return !commonDataUpToDate;
        }

        private static void UpdateAFromB(User userA, User userB)
        {
            userA.Id = userB.Id;
            userA.Name = userB.Name;
            userA.Email = userB.Email;
            userA.UpdateDate = userB.UpdateDate;
        }

        private static User MapResponseToDatabase(UsersResponse response)
        {
            return new User
            {
                Id = response.Id,
                Email = response.Email,
                Name = response.Name,
                AddDate = response.AddDate,
                UpdateDate = response.UpdateDate,
            };
        }
    }

    public class UsersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class ApiResponse<T> where T : class
    {
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public T Data { get; set; }
    }

}
