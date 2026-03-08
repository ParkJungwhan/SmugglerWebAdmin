-- Region authorization lookup tables
CREATE TABLE IF NOT EXISTS regions (
    region_code INT PRIMARY KEY,
    region_name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS user_regions (
    user_id TEXT NOT NULL,
    region_code INT NOT NULL,
    role_name TEXT NULL,
    PRIMARY KEY (user_id, region_code),
    CONSTRAINT fk_user_regions_users FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    CONSTRAINT fk_user_regions_regions FOREIGN KEY (region_code) REFERENCES regions(region_code) ON DELETE CASCADE
);

-- Seed regions (1000-range)
INSERT INTO regions (region_code, region_name) VALUES
    (1001, 'Seoul'),
    (1002, 'Tokyo'),
    (1003, 'US East'),
    (1004, 'US West')
ON CONFLICT (region_code) DO NOTHING;

-- Seed user-region mapping examples (replace user_id values as needed)
INSERT INTO user_regions (user_id, region_code, role_name) VALUES
    ('admin', 1001, 'Admin'),
    ('admin', 1002, 'Admin'),
    ('admin', 1003, 'Admin'),
    ('admin', 1004, 'Admin')
ON CONFLICT (user_id, region_code) DO NOTHING;
