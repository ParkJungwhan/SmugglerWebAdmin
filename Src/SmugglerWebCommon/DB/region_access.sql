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

-- Auto-map admin users to all seeded regions
INSERT INTO user_regions (user_id, region_code, role_name)
SELECT u.user_id, r.region_code, 'Admin'
FROM users u
CROSS JOIN regions r
WHERE UPPER(u.user_name) IN ('ADMIN', 'ADMINISTRATOR')
ON CONFLICT (user_id, region_code) DO NOTHING;

-- Auto-map every user to default region 1001 as Viewer
INSERT INTO user_regions (user_id, region_code, role_name)
SELECT u.user_id, 1001, 'Viewer'
FROM users u
ON CONFLICT (user_id, region_code) DO NOTHING;
