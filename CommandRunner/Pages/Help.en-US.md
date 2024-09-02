        ## Introduction
CommandRunner is a Windows utility for running console commands with screen reader accessible command output. This utility has been created with the aim to allow easy text selection, searching, copying and clearing for the textual output of the user provided command.

## Features
* Run console command in specified working directory with the possibility of text selection and copying of the command output.
* Command and working directory history. Up to ten history items can be chosen by pressing the Down arrow key when the command or working directory combobox is focused.
* Find text in command output. Press Control + F to show the search dialog, access the search history by pressing the Down arrow key when the find what combobox is focused, hit Enter to find the next occurrence. Successive occurrences can be found using the F3 key, press Shift + F3 for searching backward. The search may or not may be case sensitive.
* In settings, CommandRunner can be configured so that notification sound will be played whenever a given regular expression matches a text in the output line of the currently running command. This way, if CommandRunner is in background, one can be notified when a given string, such as "ERROR", occurs in new output, or when a successful compilation occurs by detecting another given string.

## Keyboard shortcuts
* Control + Enter: Runs the command and focuses the output textbox.
* Control + K: Kills the running process.
* Control + L: Focuses the command textbox.
* Control + O: Focuses the output textbox.
* Control + F: Shows the find text dialog.
* F3: Find the next text occurance.
* Shift + F3: Find the previous text occurance.
* Control + D: Clears the output textbox.
* Control + Shift + C: Copies the whole output textbox content to clipboard.

## Contact and feedback
If you have suggestions for CommandRunner improvement, problems with its functionality or other comments, you can drop me an email to [adam.samec@gmail.com](mailto:adam.samec@gmail.com)

## License
CommandRunner is available as a free and open-source software under the MIT license.

### The MIT License (MIT)

Copyright (c) 2024 Adam Samec

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
