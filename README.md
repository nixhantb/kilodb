# KiloDb

KiloDb is a small, crash-safe key-value database server , storing data
in a B Tree on disk and speaking a simple `set` / `get` / `del` / `scan` protocol over TCP.

## How it's implemented

- **CommandProcessor** parses `set` / `get` / `del` / `scan` commands.
- **KiloDatabase** wraps everything with locking, transactions, commit, and rollback.
- **BTree** stores the data on disk, handling search, insert, split, scan, and delete.
- **BufferPool** caches pages in memory (LRU) and tracks which are dirty.
- **Pager** reads and writes fixed 4 KB pages to `kilo.db`.
- **WriteAheadLog** logs changes to `kilo.wal` before they hit the data file, so a crash
  mid-write can't corrupt the database.
- **KiloServer** accepts TCP connections and handles each client on its own async task.

See [CONTRIBUTING.md](CONTRIBUTING.md) for the commit rule used in this repo.
