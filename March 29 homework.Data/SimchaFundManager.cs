using System.Data;
using System.Data.SqlClient;

namespace March_29_homework.Data
{
    public class SimchaFundManager
    {
        private string _connectionString;
        public SimchaFundManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int GetTotalContributor()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT (*) FROM Contributors";
            connection.Open();
            return (int)command.ExecuteScalar();
        }

        public Simcha GetSimchaById(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Simchas WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new Simcha
            {
                Id = id,
                Name = (string)reader["Name"],
                Date = (DateTime)reader["Date"],
                Total = GetTotalForSimcha(id),
                ContributerCount = GetContributorCountForSimcha((int)reader["Id"])
            };
        }

        public List<Simcha> GetSimchas()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Simchas";
            List<Simcha> simchas = new List<Simcha>();
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = (int)reader["Id"];
                simchas.Add(new()
                {
                    Id = id,
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    Total = GetTotalForSimcha(id),
                    ContributerCount = GetContributorCountForSimcha((int)reader["Id"])
                });
            }
            return simchas;
        }

        public int AddSimcha(Simcha s)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Simchas VALUES(@name, @date); SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@name", s.Name);
            command.Parameters.AddWithValue("@date", s.Date);
            connection.Open();
            return (int)(decimal)command.ExecuteScalar();
        }

        public List<Contributor> GetContributors()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Contributors";
            List<Contributor> contributors = new List<Contributor>();
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = (int)reader["Id"];
                contributors.Add(new()
                {
                    Id = id,
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"],
                    DateCreated = (DateTime)reader["DateCreated"],
                    Balance = GetBalance(id)
                });
            }
            return contributors;
        }

        public Contributor GetContributor(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Contributors WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new Contributor()
            {
                Id = id,
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                PhoneNumber = (string)reader["PhoneNumber"],
                AlwaysInclude = (bool)reader["AlwaysInclude"],
                Balance = GetBalance(id)
            };

        }

        public int AddContributor(Contributor c)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Contributors (FirstName, LastName, PhoneNumber, AlwaysInclude, DateCreated)
                                    VALUES(@firstName, @lastName, @phoneNumber, @alwaysInclude, @dateCreated); SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@firstName", c.FirstName);
            command.Parameters.AddWithValue("@lastName", c.LastName);
            command.Parameters.AddWithValue("@phoneNumber", c.PhoneNumber);
            command.Parameters.AddWithValue("@alwaysInclude", c.AlwaysInclude);
            command.Parameters.AddWithValue("@dateCreated", c.DateCreated);

            connection.Open();
            return (int)(decimal)command.ExecuteScalar();
        }

        public void UpdateContributor(Contributor c)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"UPDATE Contributors SET FirstName = @firstName, LastName = @lastName,
                                    PhoneNumber = @phoneNumber, AlwaysInclude = @alwaysInclude, DateCreated = @dateCreated
                                    WHERE Id = @id";
            command.Parameters.AddWithValue("@id", c.Id);
            command.Parameters.AddWithValue("@firstName", c.FirstName);
            command.Parameters.AddWithValue("@lastName", c.LastName);
            command.Parameters.AddWithValue("@phoneNumber", c.PhoneNumber);
            command.Parameters.AddWithValue("@alwaysInclude", c.AlwaysInclude);
            command.Parameters.AddWithValue("@dateCreated", c.DateCreated);

            connection.Open();

            command.ExecuteNonQuery();
        }
        public int GetTotalForSimcha(int simchaId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT SUM (AMOUNT) AS 'Total' FROM Contributions WHERE SimchaId = @simchaId";
            command.Parameters.AddWithValue("@simchaId", simchaId);
            connection.Open();
            if (command.ExecuteScalar() == DBNull.Value)
            {
                return default;
            }
            else
            {
                return (int)command.ExecuteScalar();
            }
           
        }

        public int GetContributorCountForSimcha(int simchaId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT (*) FROM Contributions WHERE SimchaId = @simchaId";
            command.Parameters.AddWithValue("@simchaId", simchaId);
            connection.Open();
            return (int)command.ExecuteScalar();
        }

        private decimal GetTotalDeposites(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT SUM (AMOUNT) AS 'Total' FROM Deposites";

            if(id > 0)
            {
                command.CommandText += " WHERE ContributorId = @id";
            }
            command.Parameters.AddWithValue("@id", id);
            connection.Open();

            if (command.ExecuteScalar() == DBNull.Value)
            {
                return default;
            }
            else
            {
                return (decimal)(int)command.ExecuteScalar();
            }
        }

        private decimal GetContributoins(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT SUM (AMOUNT) AS 'Total' FROM Contributions";
            if (id > 0)
            {
                command.CommandText += " WHERE ContributorId = @id";
            }
            command.Parameters.AddWithValue("@id", id);
            connection.Open();

            if(command.ExecuteScalar() == DBNull.Value)
            {
                return default;
            }
            else
            {
                return (decimal)(int)command.ExecuteScalar();
            }
           
        }

        public decimal GetBalance(int id)
        {
            return GetTotalDeposites(id) - GetContributoins(id);
        }

        private void DeleteContributionsForSimcha(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM Contributions WHERE SimchaId = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }

        private void AddContributions(List<Contribution> contributions, int simchaId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Contributions
                                    VALUES(@contributorId, @simchaId, @amount, @date)";
            connection.Open();
            foreach (Contribution contribution in contributions)
            {
                if (contribution.Include)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@contributorId", contribution.ContributorId);
                    command.Parameters.AddWithValue("@simchaId", simchaId);
                    command.Parameters.AddWithValue("@amount", contribution.Amount);
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateContribution(int id, List<Contribution> contributions)
        {
            DeleteContributionsForSimcha(id);
            AddContributions(contributions, id);
        }

        public Dictionary<int, int> GetContributionsForSimcha(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Contributions WHERE SimchaId = @id";
            command.Parameters.AddWithValue("@id", id);
            var simchas = new Dictionary<int, int>();
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                simchas.Add((int)reader["contributorId"], (int)reader["amount"]);
            }
            return simchas;
        }

        public void AddDeposite(int id, int amount, DateTime date)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Deposites VALUES(@contributorId, @amount, @date)";
            command.Parameters.AddWithValue("@contributorId", id);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@date", date);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public List<Actions> GetContributionsForContributor(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Contributions SC
                                    JOIN Simchas S 
                                    ON S.Id = SC.SimchaId
                                    WHERE ContributorId = @id";
            command.Parameters.AddWithValue("@id", id);
           
            List<Actions> list = new List<Actions>();
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Actions
                {
                    ContributorId = (int)reader["ContributorId"],
                    Amount = (int)reader["Amount"] / -1,
                    SimchaId = (int)reader["SimchaId"],
                    Date = (DateTime)reader["Date"],
                    SimchaName = (string)reader["Name"]
                });
            }

            return list;
        }

        public List<Actions> GetDepositsFoPerson(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Deposites WHERE ContributorId = @id";
            command.Parameters.AddWithValue("@id", id);
            List<Actions> list = new List<Actions>();
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Actions
                {
                    ContributorId = (int)reader["ContributorId"],
                    Amount = (int)reader["Amount"],
                    Date = (DateTime)reader["Date"]
                });
            }

            return list;
        }

        public void ContributeForAlwaysInclude(List<Contributor> contributors, int simchaId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Contributions
                                    VALUES(@contributorId, @simchaId, @amount, @date)";
            connection.Open();
            foreach (Contributor c  in contributors)
            {
                if (c.AlwaysInclude)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@contributorId", c.Id);
                    command.Parameters.AddWithValue("@simchaId", simchaId);
                    command.Parameters.AddWithValue("@amount", 5);
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public void ContributeToAllUpcomingSimchas(List<Simcha> simchas, int contId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Contributions
                                    VALUES(@contributorId, @simchaId, @amount, @date)";
            connection.Open();
            foreach (Simcha s in simchas)
            {
                if(s.Date.Date > DateTime.Now.Date)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@contributorId", contId);
                    command.Parameters.AddWithValue("@simchaId", s.Id);
                    command.Parameters.AddWithValue("@amount", 5);
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                    command.ExecuteNonQuery();
                }
                    
            }
        }
    }
}