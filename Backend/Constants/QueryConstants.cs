namespace TodoApp.Constants;

public static class QueryConstants
{
    public static class Tasks
    {
        public const string AddQuery = "INSERT INTO tasks (title, description, status, priority, user_id) VALUES (@title, @description, @status, @priority, @user_id) RETURNING id;";
        public const string DeleteQuery = "DELETE FROM tasks WHERE id = @id AND user_id = @user_id;";
        public const string GetQuery = "SELECT * FROM tasks WHERE user_id = @user_id";
        public const string StatusQuery = "SELECT * FROM tasks WHERE status = @status";
        public const string UpdateQuery = @" UPDATE tasks SET title = @title, description = @description, status = @status, priority = @priority WHERE id = @id AND  user_id = @user_id;";
    }

    public static class Users
    {
        public const string CreateQuery = "INSERT INTO users (username, password_hash) VALUES ( @username, @password_hash)";
        public const string DeleteQuery = "DELETE FROM users WHERE id = @id";
        public const string GetByIdQuery = "SELECT * FROM users WHERE  id = @id";
        public const string GetByUsernameQuery = "SELECT * FROM users WHERE username = @username";
    }
}