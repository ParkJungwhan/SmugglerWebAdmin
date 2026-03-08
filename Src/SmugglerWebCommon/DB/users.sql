-- Identity-compatible users table for DapperUserStore
CREATE TABLE IF NOT EXISTS users (
    id TEXT PRIMARY KEY,
    user_name TEXT NULL,
    normalized_user_name TEXT NULL,
    email TEXT NULL,
    normalized_email TEXT NULL,
    email_confirmed BOOLEAN NOT NULL DEFAULT FALSE,
    password_hash TEXT NULL,
    security_stamp TEXT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_users_normalized_user_name
    ON users (normalized_user_name)
    WHERE normalized_user_name IS NOT NULL;

CREATE UNIQUE INDEX IF NOT EXISTS ux_users_normalized_email
    ON users (normalized_email)
    WHERE normalized_email IS NOT NULL;
