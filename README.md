# Lotusctl

![Banner](https://media.discordapp.net/attachments/946013429200723989/1043883049642885251/Frame_2.png)

A simple systemd service manager.
I made this app because I was too lazy to write `systemctl restart ...` again and again.
I know using the terminal is cool but sometimes it gets tiring.

## 🖼️ Preview

![The App](https://media.discordapp.net/attachments/946013429200723989/1043829198088847420/image.png)

## 📥 Installation

Requirements:

-   GNU Linux OS (floss)
    -   Systemd init system
    -   .NET 6.0 Runtime
-   Windows  
    This shit already have `services.msc`

### 🙂 Easy Installation

`install.sh` is comming soon.

### 💪 Manual Installation

1.  Download the [lotusctl.tar.xz](https://github.com/BayuDC/lotusctl/releases)
2.  Extract it
    ```
    $ tar -xf lotusctl.tar.xz
    ```
3.  Copy it
    ```
    $ sudo cp -r ./lotusctl /opt
    ```
4.  Link the binary
    ```
    $ sudo ln -s /opt/lotusctl/lotusctl /usr/bin/lotusctl
    ```
5.  Run it
    ```
    $ lotusctl
    ```
6.  Create desktop shorcut (optional)
    ```
    $ sudo cp ./lotusctl.desktop /usr/share/applications
    ```

## 🔧 Configuration

Just modify the `~/.config/lotusctl/services.scv`.

-   `codeName` is the service name
-   `displayName` is optional

###

Example:

```
codeName,displayName
apache2,Apache
nginx,Nginx
mariadb,MariaDB
postgresql,PostgreSQL
docker,
```

## 🚧 Development

1.  Clone the project
    ```
    $ git clone https://github.com/BayuDC/lotusctl.git
    $ cd lotusctl
    ```
2.  Install Dependencies
    ```
    $ dotnet restore
    ```
3.  Compile and Run
    ```
    $ dotnet run
    ```

## 📑 Todo

-   App icon
-   Context menu
-   `install.sh`
-   `uninstall.sh`
