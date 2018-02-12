# APPREFINE Mattermost Bots

Hello there! Welcome to my small collection of simple yet useful Mattermost bots. I will gladly accept some additional PRs :)

This repo is MIT licensed. Do what you want with the code.

# SkypeBot

This bot introduces slash commands through which you can quickly initiate Skype for Business group conversations with members of your channel.

## How to install:
- in Mattermost admin settings enable Slash commands integration and personal access tokens
- as an admin user in your profile settings generate a new token for the bot
- deploy SkypeBot .NET Core web application to somewhere
- fill its `appsettings.json` using the token you just generated
- in Mattermost integration settings create a new slash command with "skype" trigger word. Use "SKYPE_BOT_URL/api/slash" for the request URL, where SKYPE_BOT_URL is the address of the SkypeBot .NET Core app accessible from the Mattermost server.

> TIP: If you deployed the app directly to the Mattermost service server and thus your SKYPE_BOT_URL mentioned above is "localhost" or "127.0.0.1" - ensure that you also have "localhost" or "127.0.0.1" in "Allow untrusted internal connections to:" setting inside Advanced -> Developer tab of Mattermost admin settings.

In the future the process will be simplified and the documentation expanded.

## Usage:

The currently supported slash commands are:
- `/skype id SKYPE_ID` - each user should run this to set their skype ids (e.g. `user@domain.example.com`)
- `/skype meeting` - creates a Skype for Business group conversation with members of your channel