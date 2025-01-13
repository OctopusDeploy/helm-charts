---
"kubernetes-agent": patch
---

Updated Tentacle to version 8.2.2585, which increases the maximum buffer size for the output scanner in the bootstrap runner. This fix addresses an issue where output lines longer than the previous buffer limit would fail to be written.
