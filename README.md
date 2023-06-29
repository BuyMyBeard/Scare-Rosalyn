# Scare Rosalyn!
**Learn You a Game Jam: Pixel Edition submission**

[Download the game here](https://github.com/BuyMyBeard/Scare-Rosalyn/releases/tag/v1.1.1) (Better experience) or
[Play here in the browser!](https://buymybeard.github.io/Scare-Rosalyn/) (don't forget to scroll down and press the fullscreen button)

![thumbnail](https://github.com/BuyMyBeard/Scare-Rosalyn/assets/95039323/9a399de1-2fc5-4ae5-9437-c1e72e44d12c)

[itch.io Page!](https://buymybeard.itch.io/scare-rosalyn)

This Game jam done solo, except for the help of **Andre Roz** for providing me the sound effects.
I tried to get out of my confort zone for this jam and make a 3d game, and this was also an initiation to modelling and voxel art for me.

## Implementation Challenge
Since I never programmed a 3d game before, I had to face a lot of challenges here. Producing all the art assets for the game seemed like
a tremendous undertaking, since I had to get used to modelling and using new software. It was actually a lot simpler than I thought it would be.
I used a piece of software named Voxedit, which is a very decent tool to make voxel art. It seems to have marketed to the NFT craze though, which
I find unfortunate. I was able with the tool to make a rig for both my character models, and then import them in Unity in DAE format to then be able to 
animate them. Most of the props were built with this tool, and I used ProBuilder to make the house and garage models.

The bigger challenge was actually to do with scope and programming a complex AI. During the project, I was focusing a bit too much on scalability
for some of the systems in the game (this happened with the button prompt system notably). I realized later that I should have focused on velocity for this
project, since it was in the context of a jam.

I say that, but I think making the AI more scalable would have made it easier to debug. I definitely want to get familiar with the _finite state machine_
design pattern to manage state.


## Game Design Challenge
I encountered a lot of design problems in my game, since I like to be creative and create games with ideas from different genres never combined before.
I think the AI for this game is serviceable, but could have been improved much more. It goes a bit at odds with the gameplay loop of the game though, and
that is why I iterated a few ideas for it's behaviour. 

Currently, it will always try and complete the objectives, but it used to run away from the player when you scared it. It was a problem, because the lose
condition for the game was to let the AI complete all the objectives. Being able to constantly scare her and making her pick a position to run away to,
then selecting a random objective to complete meant that you could possibly just camp an objective, or get in her face and prevent her progress.

I think there is value in the ideas this game brings to the table, being most notably the interactive environment in the style of _Hitman_ games. You can,
for example, grab the TV remote, and then use it to scare Rosalyn when she walks by the TV or sits on the couch by turning it on out of nowhere. You can also
enter the car and wait for Rosalyn, which is an insta win if you find it. _Hitman_ has stuff like this, but it's usually more convoluted and involved.
I would have liked to push this idea further, but for scope reasons this was impossible. 

Anyhow, enjoy browsing my code for the game!
