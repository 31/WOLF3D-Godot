Everything in this repo is still a work in progress: **the game is not playable yet.**

# Goals

This project aims to re-create the experience of the classic 1992 first person shooter game Wolfenstein 3D and other games running on the same engine (Spear of Destiny and Super 3D Noah's Ark) for virtual reality, ultimately to make these old games playable on the Oculus Quest in true stereoscopic 3D.

There has already been [an admirable effort at recreating Wolfenstein 3D in VR](https://further-beyond.itch.io/wolf3dvr) so anyone who just wants to play the game in VR right now on their PC should check that version out. However, in my view, that version leaves some gaps which this project aims to fill in:

1. This is on the Godot-Mono engine, so the entire technology stack is as open source as possible. This remains true despite the fact that almost none of the original Wolfenstein 3D code is directly involved in this project. It's a port, but it's not a source port: it's a rough approximation of the game's rules re-interpreted for VR.
2. This version is going to load in all the assets from the original 1992 game files at runtime with no intermediary formats. (beyond adding some XML for things normally compiled into the EXE) If you can't auto-download the shareware or otherwise obtain the original MS-DOS game files then you can't play.
3. Apart from rendering in HD, this version is going to keep the aesthetics strictly matching the original 1992 MS-DOS PC version to a ridiculously autistic and even slightly creepy degree. This means emulated Adlib / Sound Blaster sound, no dynamic lighting and no high resolution textures. If you don't like the original pixel art by Adrian Carmack and the original Adlib soundtrack by Robert Prince then this is not the version of Wolfenstein 3D for you.
4. This version is going to have extensive mod support, including directly supporting classic mods and user-made map packs from the original game and/or made with existing modding tools from the community. In fact, the whole thing is being constructed from the beginning such that everything (even the full registered Wolf3D) is treated during development as a mod of the shareware version. Mods that require patching WOLF3D.EXE will probably not run, but most mods will probably run.
5. The goal is to make this program work on the Oculus Quest. Other platforms, like PC VR, may also be supported, but the Oculus Quest is the main goal.

# Thanks

This is my (Ben McLean's) pet project, but I have received much help that I need to thank people for.

First of all, [this is mind-blowing](https://bitbucket.org/NY00123/gamesrc-ver-recreation/). I still have a hard time believing tbat anything so awesome could ever exist.

Second, Val�ry Sablonni�re has been a huge help with his [C# port of the DOSBOX OPL (Adlib / Sound Blaster) emulator](https://github.com/scemino/NScumm.Audio). I might not have been able to get the sound working in Godot without his code, which is GPL by the way.

Third, [Adam Biser](https://adambiser.itch.io/wdc), [Fabien Sanglard](http://fabiensanglard.net/gebbwolf3d/), [Blzut3](http://maniacsvault.net/ecwolf/) and even John Carmack himsself have been quite helpful, not only by having made significant contributions to the Wolfenstein 3D fan community but also by directly answering some of my emailed technical questions. Also, the [Game Modding wiki](http://www.shikadi.net/moddingwiki/Wolfenstein_3-D) has been quite helpful as a resource.

[kalbert312](https://github.com/kalbert312) helped me figure the sprite graphics format out by sending me some of his Java and TypeScript code to translate into C#.

Finally, gotta thank the original id Software team from back in the day for making such awesome games!