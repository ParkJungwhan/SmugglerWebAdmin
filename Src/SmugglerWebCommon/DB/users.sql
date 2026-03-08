-- users table aligned with project DB naming convention
CREATE TABLE IF NOT EXISTS users (
    user_id TEXT PRIMARY KEY,
    user_name TEXT NOT NULL,
    user_email TEXT NOT NULL,
    user_ps TEXT NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_users_user_name_upper
    ON users (UPPER(user_name));

CREATE UNIQUE INDEX IF NOT EXISTS ux_users_user_email_upper
    ON users (UPPER(user_email));
