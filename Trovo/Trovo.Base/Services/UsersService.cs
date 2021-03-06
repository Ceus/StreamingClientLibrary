﻿using Newtonsoft.Json.Linq;
using StreamingClient.Base.Util;
using StreamingClient.Base.Web;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trovo.Base.Models.Users;

namespace Trovo.Base.Services
{
    public class UsersService : TrovoServiceBase
    {
        private class GetUsersResult
        {
            public long total { get; set; }
            public List<UserModel> users { get; set; } = new List<UserModel>();
        }

        /// <summary>
        /// Creates an instance of the UsersService.
        /// </summary>
        /// <param name="connection">The Trovo connection to use</param>
        public UsersService(TrovoConnection connection) : base(connection) { }

        public async Task<PrivateUserModel> GetCurrentUser()
        {
            return await this.GetAsync<PrivateUserModel>("getuserinfo");
        }

        public async Task<UserModel> GetUser(string username)
        {
            Validator.ValidateString(username, "username");
            IEnumerable<UserModel> users = await this.GetUsers(new List<string>() { username });
            return (users != null) ? users.FirstOrDefault() : null;
        }

        public async Task<IEnumerable<UserModel>> GetUsers(IEnumerable<string> usernames)
        {
            Validator.ValidateList(usernames, "usernames");

            JObject jobj = new JObject();
            JArray jarr = new JArray();
            foreach (string username in usernames)
            {
                jarr.Add(username);
            }
            jobj["user"] = jarr;

            GetUsersResult result = await this.PostAsync<GetUsersResult>("getusers", AdvancedHttpClient.CreateContentFromObject(jobj));
            if (result != null)
            {
                return result.users;
            }
            return null;
        }
    }
}
