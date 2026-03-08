-- Region authorization lookup tables
CREATE TABLE IF NOT EXISTS regions (
    region_code INT PRIMARY KEY,
    region_name TEXT NOT NULL,
    address TEXT NULL
);

ALTER TABLE regions
ADD COLUMN IF NOT EXISTS address TEXT NULL;

CREATE TABLE IF NOT EXISTS user_regions (
    user_id TEXT NOT NULL,
    region_code INT NOT NULL,
    role_name TEXT NULL,
    PRIMARY KEY (user_id, region_code),
    CONSTRAINT fk_user_regions_users FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    CONSTRAINT fk_user_regions_regions FOREIGN KEY (region_code) REFERENCES regions(region_code) ON DELETE CASCADE
);

-- Seed regions (1000-range)
INSERT INTO regions (region_code, region_name, address) VALUES
    (1001, 'Seoul', 'https://localhost:7152/entry'),
    (1002, 'Tokyo', 'https://localhost:7152/entry'),
    (1003, 'US East', 'https://localhost:7152/entry'),
    (1004, 'US West', 'https://localhost:7152/entry')
ON CONFLICT (region_code) DO UPDATE
SET region_name = EXCLUDED.region_name,
    address = EXCLUDED.address;

-- Auto-map admin users to all seeded regions (admintype != 0)
INSERT INTO user_regions (user_id, region_code, role_name)
SELECT u.user_id, r.region_code, CONCAT('Admin-', u.admintype)
FROM users u
CROSS JOIN regions r
WHERE u.admintype <> 0
ON CONFLICT (user_id, region_code) DO NOTHING;

-- Auto-map every user to default region 1001 as Viewer
INSERT INTO user_regions (user_id, region_code, role_name)
SELECT u.user_id, 1001, 'Viewer'
FROM users u
ON CONFLICT (user_id, region_code) DO NOTHING;
