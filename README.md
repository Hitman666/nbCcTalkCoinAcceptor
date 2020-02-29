nbCcTalkCoinAcceptor
====================

This project contains usage instructions and code examples (C#) for communication with coin acceptor using the [ccTalk](http://cctalk.org) protocol.

This project is based on [other ccTalk project](https://code.google.com/p/cctalk-net/) which has been discontinued. API is based on ccTalk generic 
specification issue 4.6 as specified on the official ccTalk website (included in the documentation folder: _cctalkPart2v4.6.pdf_). It is a basic C# .NET assembly, built for 4.0 framework. Although the project on which this one is based has support for bill validators they have not been tested and the example
WinForms application demostrates only the usage for coin acceptor.

The main reason this project was made is because the aforementioned projects didn't work "out of the box" and didn't include enough documentation to make an easy start. Also, the coin
acceptor didn't have full implementation for basic usage (for example, the initial coin inhibition setting was missing).

The project has three folders:
* **Documentation** - project documentation in [PDF](Documentation/nbCcTalkCoinAcceptor.pdf) and [ODT](Documentation/nbCcTalkCoinAcceptor.odt) format, and PDF format of [ccTalk specification version 4.6](Documentation/cctalkPart2v4.6.pdf) 
* **nbCcTalkCoinAcceptor_exe** - compiled WinForms application for testing with Euro currency
* **nbCcTalkCoinAcceptor_VsProject** - whole VisualStudio project

##Getting started
Please refer to the full documentation file [nbCcTalkCoinAcceptor.pdf](Documentation/nbCcTalkCoinAcceptor.pdf) in the Documentation folder.

License: [MIT License
](http://www.opensource.org/licenses/mit-license.php). _although, I really think that Randall said it all in [this post](https://www.rdegges.com/2016/i-dont-give-a-shit-about-licensing/)_
