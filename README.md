# APPREFINE Mattermost Bots

Hello there! Welcome to my small collection of simple yet useful Mattermost bots. I will gladly accept some additional PRs :)

This repo is MIT licensed. Do what you want with the code.

# Common info

## How to install:
- in Mattermost admin settings enable Slash commands integration and personal access tokens
- as an admin user in your profile settings generate a new token for the bot
- deploy bot .NET Core web application to somewhere
- fill its `appsettings.json` using the token you just generated
- in Mattermost integration settings create a new slash command with a trigger word correct for a bot. Use "BOT_URL/api/slash" for the request URL, where BOT_URL is the address of the bot .NET Core app accessible from the Mattermost server.

> TIP: If you deployed the app directly to the Mattermost service server and thus your BOT_URL mentioned above is "localhost" or "127.0.0.1" - ensure that you also have "localhost" or "127.0.0.1" in "Allow untrusted internal connections to:" setting inside Advanced -> Developer tab of Mattermost admin settings.

In the future the process will be simplified and the documentation expanded.

## Development:

I recommend using `mattermost-preview` official docker container during development.
You can run the bot on your host machine and use your bridge network adapter's IP inside Mattermost to communicate with it.

# SkypeBot

This bot introduces slash commands through which you can quickly initiate Skype for Business group conversations with members of your channel.

## Usage:

The following commands are available ([TEXT] denotes optional parameters):
- `/skype id YOUR_ID`  - saves your skype id. Usually this looks like an email.
- `/skype meeting [GROUP]` - opens a Skype group conversation window with memebers of your channel. If GROUP is supplied the members are also filtered by Skype group (see commands belowe).
- `/skype join GROUP` - assignes you to a group. Not case sensitive.
- `/skype leave GROUP` - removes you from a Skype group

# PollBot

This bot add slash commands for creating open and close answered polls.
Usage (brackets denote optional parameters):
- `/poll answer YOUR ANSWER`. This will only work if there's an open poll in the channel. You can create new polls with `/poll new`.
- `/poll new open DESCRIPTION` - creates a new open poll. "Open" means that users can give text answers.
- `/poll new closed ANSWERS DESCRIPTION` - NOT IMPLEMENTED
- `/poll results [ID]` - displays results of a poll. If ID is supplied - a specific poll, otherwise latest.
