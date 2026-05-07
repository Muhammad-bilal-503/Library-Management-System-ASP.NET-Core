-- =============================================
-- BookVault Library Management System
-- Database Script (SQLite compatible)
-- Note: EF Core auto-creates this on startup
-- =============================================

-- Books Table
CREATE TABLE IF NOT EXISTS Books (
    Id                INTEGER PRIMARY KEY AUTOINCREMENT,
    Title             TEXT    NOT NULL,
    Author            TEXT    NOT NULL,
    ISBN              TEXT    NOT NULL,
    Genre             TEXT,
    Publisher         TEXT,
    PublishedYear     INTEGER,
    TotalCopies       INTEGER NOT NULL DEFAULT 1,
    AvailableCopies   INTEGER NOT NULL DEFAULT 1,
    Description       TEXT,
    CoverImageUrl     TEXT,
    CoverImageId      TEXT,
    AddedDate         TEXT    NOT NULL DEFAULT (datetime('now'))
);

-- Members Table
CREATE TABLE IF NOT EXISTS Members (
    Id                INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName          TEXT    NOT NULL,
    Email             TEXT    NOT NULL,
    Phone             TEXT,
    MembershipId      TEXT    NOT NULL,
    Address           TEXT,
    MembershipType    INTEGER NOT NULL DEFAULT 0,
    MembershipExpiry  TEXT    NOT NULL,
    IsActive          INTEGER NOT NULL DEFAULT 1,
    CreatedAt         TEXT    NOT NULL DEFAULT (datetime('now'))
);

-- Loans Table
CREATE TABLE IF NOT EXISTS Loans (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
    BookId      INTEGER NOT NULL,
    MemberId    INTEGER NOT NULL,
    IssueDate   TEXT    NOT NULL DEFAULT (datetime('now')),
    DueDate     TEXT    NOT NULL,
    ReturnDate  TEXT,
    IsReturned  INTEGER NOT NULL DEFAULT 0,
    Notes       TEXT,
    FOREIGN KEY (BookId)   REFERENCES Books(Id),
    FOREIGN KEY (MemberId) REFERENCES Members(Id)
);

-- =============================================
-- Sample Data
-- =============================================

INSERT INTO Books (Title, Author, ISBN, Genre, Publisher, PublishedYear, TotalCopies, AvailableCopies, Description)
VALUES
    ('Clean Code',              'Robert C. Martin', '978-0132350884', 'Technology', 'Prentice Hall',  2008, 3, 3, 'A handbook of agile software craftsmanship.'),
    ('The Pragmatic Programmer','Andrew Hunt',       '978-0201616224', 'Technology', 'Addison-Wesley', 1999, 2, 2, 'Your journey to mastery.'),
    ('Design Patterns',         'Gang of Four',      '978-0201633610', 'Technology', 'Addison-Wesley', 1994, 2, 2, 'Elements of Reusable Object-Oriented Software.'),
    ('Sapiens',                 'Yuval Noah Harari', '978-0062316097', 'History',    'Harper',          2011, 4, 4, 'A brief history of humankind.'),
    ('Atomic Habits',           'James Clear',       '978-0735211292', 'Self-Help',  'Avery',           2018, 3, 3, 'Tiny changes, remarkable results.');

INSERT INTO Members (FullName, Email, Phone, MembershipId, Address, MembershipType, MembershipExpiry, IsActive)
VALUES
    ('Ahmed Khan',   'ahmed@example.com',  '0300-1234567', 'MEM-001', 'Block 5, Islamabad', 1, '2027-01-01', 1),
    ('Fatima Malik', 'fatima@example.com', '0321-9876543', 'MEM-002', 'F-7, Islamabad',     2, '2028-01-01', 1);
