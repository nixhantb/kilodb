# Commit rule

Every commit message must start with an incrementing ticket number:

```
KILO-<number>: <message>
```

- `<number>` increments by 1 from the last `KILO-<number>` in history (zero-padded to 2 digits, e.g. `KILO-06`).
- `: ` must follow immediately after the number.
- Example: `KILO-06: add BufferPool LRU eviction`

This is enforced by git hooks checked into `.githooks/`:

- `commit-msg` - rejects a commit whose message doesn't match the rule or is out of sequence.
- `pre-push` - rejects a push if any outgoing commit doesn't match the rule (catches `--no-verify` commits).

## One-time setup (per clone)

```sh
git config core.hooksPath .githooks
```

On Windows, run this from Git Bash / PowerShell with Git for Windows installed; the hooks are POSIX shell scripts and run via Git's bundled `sh`.
