# Git Workflow

### Git Branching Model

> Never commit to `master` or `dev`!

We will have two branches: `master` and `dev`.

The `master` branch will contain only the most stable version of the game, one that is confirmed to work.  
The `dev` branch will be the working version, in that it will be where we branch from and merge to when we are working normally.

When a feature or a release is to be made the `dev` branch will be merged into `master` after testing.

During normal development, the `dev` branch will be the default branch, and we will have feature branches that come off of it.

For example, if we wanted to add jumping to the game. We would start at the `dev` branch and use `git pull origin dev`
to ensure it was up to date. Then we make the feature branch: `git checkout -b add_jumping`. We are now working on
the feature branch `add_jumping`.

Then, the feature will be added, the code tested, and finally, it will be added(`git add <files>`) and then finally, committed.

When committing, we will follow the following style(using the example):

>Add jump ability to player.  
>
> * Changed the way movement was handled to allow for jumping.
> * Added rigid body to the player model to allow for physics interactions.

Once you are happy with your feature and you wish to merge it into `dev` you then push the feature branch.

`git push origin add_jumping`

Now you can go the repository online and create a pull request.

The pull request will be reviewed and once it has been successfully been reviewed and the adequate changes have been made,
it will be given a :+1:, this is your indication that the pull request can be merged. Once it is merged, you should delete
your branch online, checkout `dev` on your machine and then pull the changes.


### Assets

I think it'd be a good idea to try and keep visual and audio assets synced with dropbox instead of git as that will
make the `.git` file huge. Although the implementation of that will have to be seen later.
