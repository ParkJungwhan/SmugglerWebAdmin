-- Region authorization lookup tables
CREATE TABLE IF NOT EXISTS regions (
    region_code TEXT PRIMARY KEY,
    region_name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS user_regions (
    user_id TEXT NOT NULL,
    region_code TEXT NOT NULL,
    role_name TEXT NULL,
    PRIMARY KEY (user_id, region_code),
    CONSTRAINT fk_user_regions_users FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    CONSTRAINT fk_user_regions_regions FOREIGN KEY (region_code) REFERENCES regions(region_code) ON DELETE CASCADE
);
