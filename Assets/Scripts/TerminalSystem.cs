using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerminalSystem 
{
    public string CurrentUser      { get; private set; } = "sysadmin";
    public string CurrentDirectory { get; private set; } = "/home/sysadmin";
    public bool   IsRoot           { get; private set; } = false;

    private const string SudoPassword = "netlab123";

    private Dictionary<string, List<string>> fileSystem;
    private Dictionary<string, string> fileContents    = new Dictionary<string, string>();
    private Dictionary<string, string> fileOwners      = new Dictionary<string, string>();
    private Dictionary<string, string> filePermissions = new Dictionary<string, string>();

    public TerminalSystem()
    {
        InitFileSystem();
    }

    public bool EsperandoPasswordSSH() => esperandoPasswordSSH;

    void InitFileSystem()
    {
        fileSystem = new Dictionary<string, List<string>>
        {
            { "/",                                           new List<string> { "bin","dev","etc","home","tmp","var" } },
            { "/bin",                                        new List<string>() },
            { "/dev",                                        new List<string> { "zero","pts" } },
            { "/dev/pts",                                    new List<string> { "0" } },
            { "/etc",                                        new List<string> { "passwd" } },
            { "/home",                                       new List<string> { "sysadmin" } },
            { "/home/sysadmin",                              new List<string> { "Desktop","Documents","Downloads","Music","Pictures","Public","Templates","Videos" } },
            { "/home/sysadmin/Desktop",                      new List<string>() },
            { "/home/sysadmin/Documents",                    new List<string> { "School","Work","adjectives.txt","alpha-first.txt","alpha-second.txt","alpha-third.txt","alpha.txt","animals.txt","food.txt","hello.sh","hidden.txt","letters.txt","linux.txt","longfile.txt","newhome.txt","numbers.txt","os.csv","people.csv","profile.txt","red.txt" } },
            { "/home/sysadmin/Documents/School",             new List<string> { "Art","Engineering","Math" } },
            { "/home/sysadmin/Documents/School/Art",         new List<string>() },
            { "/home/sysadmin/Documents/School/Engineering", new List<string>() },
            { "/home/sysadmin/Documents/School/Math",        new List<string>() },
            { "/home/sysadmin/Documents/Work",               new List<string>() },
            { "/home/sysadmin/Downloads",                    new List<string>() },
            { "/home/sysadmin/Music",                        new List<string>() },
            { "/home/sysadmin/Pictures",                     new List<string>() },
            { "/home/sysadmin/Public",                       new List<string>() },
            { "/home/sysadmin/Templates",                    new List<string>() },
            { "/home/sysadmin/Videos",                       new List<string>() },
            { "/tmp",                                        new List<string>() },
            { "/var",                                        new List<string> { "log" } },
            { "/var/log",                                    new List<string> { "alternatives.log","apache2","apt","auth.log","bootstrap.log","btmp","cron.log","dmesg","dpkg.log","faillog","fsck","kern.log","lastlog","syslog","upstart","wtmp" } },
            { "/var/log/apache2",                            new List<string>() },
            { "/var/log/apt",                                new List<string>() },
            { "/var/log/fsck",                               new List<string>() },
            { "/var/log/upstart",                            new List<string>() },
            { "/usr",                                        new List<string> { "share" } },
            { "/usr/share",                                  new List<string> { "wordlists" } },
            { "/usr/share/wordlists",                        new List<string> { "rockyou.txt", "common.txt" } },
        };

        string[] dirs = {
            "/home/sysadmin/Desktop","/home/sysadmin/Documents","/home/sysadmin/Downloads",
            "/home/sysadmin/Music","/home/sysadmin/Pictures","/home/sysadmin/Public",
            "/home/sysadmin/Templates","/home/sysadmin/Videos",
            "/home/sysadmin/Documents/School","/home/sysadmin/Documents/School/Art",
            "/home/sysadmin/Documents/School/Engineering","/home/sysadmin/Documents/School/Math",
            "/home/sysadmin/Documents/Work","/var/log/apache2","/var/log/apt",
            "/var/log/fsck","/var/log/upstart"
        };
        foreach (var d in dirs) { fileOwners[d] = "sysadmin"; filePermissions[d] = "drwx------"; }

        InitPermissionsAndOwners();
        InitFileContents();
    }

    void InitPermissionsAndOwners()
    {
        var docFiles = new[] {
            "adjectives.txt","alpha-first.txt","alpha-second.txt","alpha-third.txt","alpha.txt",
            "animals.txt","food.txt","hidden.txt","letters.txt","linux.txt","longfile.txt",
            "newhome.txt","numbers.txt","os.csv","people.csv","profile.txt","red.txt"
        };
        foreach (var f in docFiles)
        {
            string p = "/home/sysadmin/Documents/" + f;
            fileOwners[p] = "sysadmin"; filePermissions[p] = "-rw-r--r--";
        }
        fileOwners["/home/sysadmin/Documents/hello.sh"]      = "sysadmin";
        filePermissions["/home/sysadmin/Documents/hello.sh"] = "-rw-r--r--";

        var logFiles  = new[] { "alternatives.log","auth.log","bootstrap.log","btmp","cron.log","dmesg","dpkg.log","faillog","kern.log","lastlog","syslog","wtmp" };
        var logOwners = new[] { "root","syslog","root","utmp","syslog","root","root","root","syslog","utmp","syslog","utmp" };
        for (int i = 0; i < logFiles.Length; i++)
        {
            string p = "/var/log/" + logFiles[i];
            fileOwners[p]      = logOwners[i];
            filePermissions[p] = "-rw-r-----";
        }
        fileOwners["/etc/passwd"]      = "root";
        filePermissions["/etc/passwd"] = "-rw-r--r--";
    }

    void InitFileContents()
    {
        fileContents["/home/sysadmin/Documents/animals.txt"]      = "1 retriever\n2 badger\n3 bat\n4 wolf\n5 eagle";

        fileContents["/home/sysadmin/Documents/alpha.txt"]        =
            "A is for Apple\nB is for Bear\nC is for Cat\nD is for Dog\nE is for Elephant\n" +
            "F is for Flower\nG is for Grapes\nH is for Happy\nI is for Ink\nJ is for Juice\n" +
            "K is for Kangaroo\nL is for Lol\nM is for Monkey\nN is for Nickel\nO is for Oval\n" +
            "P is for Pickle\nQ is for Quark\nR is for Rat\nS is for Sloth\nT is for Turnip\n" +
            "U is for Up\nV is for Velvet\nW is for Walrus\nX is for Xenon\nY is for Yellow\nZ is for Zebra";
            
        fileContents["/home/sysadmin/Documents/alpha-first.txt"]  = "A is for Animal\nB is for Bear\nC is for Cat\nD is for Dog\nE is for Elephant\nF is for Flower";
        fileContents["/home/sysadmin/Documents/alpha-second.txt"] = "G is for Grapes\nH is for Happy\nI is for Ink\nJ is for Juice\nK is for Kangaroo\nL is for Lol";
        fileContents["/home/sysadmin/Documents/alpha-third.txt"]  = "M is for Monkey\nN is for Nickel\nO is for Oval\nP is for Pickle\nQ is for Quark\nR is for Rat";
        fileContents["/home/sysadmin/Documents/red.txt"]          = "red\nreef\nrot\nreeed\nrd\nrod\nroof\nreed\nroot\nreel\nread";
        fileContents["/home/sysadmin/Documents/profile.txt"]      = "Hello my name is Joe.\nI am 37 years old.\n3121991\nMy favorite food is avocados.\nI have 2 dogs.\n123456789101112";
        fileContents["/home/sysadmin/Documents/hello.sh"]         = "#!/bin/bash\necho \"Hello World!\"";
        fileContents["/home/sysadmin/Documents/linux.txt"]        = "Linux is a free and open-source operating system kernel.";
        fileContents["/home/sysadmin/Documents/food.txt"]         = "pizza\nburger\ntacos\nsushi\npasta";
        fileContents["/home/sysadmin/Documents/numbers.txt"]      = "1\n2\n3\n4\n5\n6\n7\n8\n9\n10";
        fileContents["/home/sysadmin/Documents/letters.txt"]      = "a\nb\nc\nd\ne\nf\ng\nh\ni\nj";
        fileContents["/home/sysadmin/Documents/adjectives.txt"]   = "happy\nsad\nfast\nslow\nbig\nsmall\nbright\ndark";
        fileContents["/home/sysadmin/Documents/os.csv"]           = "Name,Version,Year\nLinux,5.15,2021\nWindows,11,2021\nmacOS,12,2021";
        fileContents["/home/sysadmin/Documents/people.csv"]       = "Name,Age,City\nAlice,30,New York\nBob,25,Los Angeles\nCarol,35,Chicago";
        fileContents["/home/sysadmin/Documents/newhome.txt"]      = "Welcome to your new home directory.";
        fileContents["/home/sysadmin/Documents/hidden.txt"]       = "This file contains hidden information.";
        fileContents["/home/sysadmin/Documents/longfile.txt"]     =
            string.Join("\n", Enumerable.Range(1, 200).Select(i => $"Line {i}: Lorem ipsum dolor sit amet."));

        fileContents["/etc/passwd"] =
            "root:x:0:0:root:/root:/bin/bash\n" +
            "daemon:x:1:1:daemon:/usr/sbin:/usr/sbin/nologin\n" +
            "bin:x:2:2:bin:/bin:/usr/sbin/nologin\n" +
            "operator:x:1000:37::/root:\n" +
            "sysadmin:x:1001:1001:System Administrator,,,,:/home/sysadmin:/bin/bash";

        fileContents["/usr/share/wordlists/rockyou.txt"] =
            "123456\npassword\n12345678\nqwerty\n123456789\n" +
            "12345\n1234567\nletmein\n1234567890\ndragon\n" +
            "baseball\nbatman123\niloveyou\nmaster\nsunshine\nmonkey\n" +
            "welcome\nshadow\npassword123\nadmin\nlogin";

        fileContents["/usr/share/wordlists/common.txt"] =
            "admin\nroot\nuser\ntest\nguest\npassword\n" +
            "123456\nletmein\nqwerty\nabc123";
    }

    public string Execute(string raw)
    {
        raw = raw.Trim();

        // Si estamos esperando la contraseña SSH 
        if (esperandoPasswordSSH)
        return ProcesarPasswordSSH(raw);

        string[] tokens = raw.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0) return "";

        string   cmd    = tokens[0].ToLower();
        string[] args   = tokens.Skip(1).ToArray();
        bool     isSudo = false;

        if (cmd == "sudo" && args.Length > 0)
        {
            isSudo = true;
            cmd    = args[0];
            args   = args.Skip(1).ToArray();
        }

        switch (cmd)
        {
            case "ifconfig":   return CmdIfconfig(args); 
            case "clear":      return "%%CLEAR%%"; 
            case "ls":         return CmdLs(args);
            case "mkdir":      return CmdMkdir(args);
            case "touch":      return CmdTouch(args);
            case "echo":       return CmdEcho(raw);
            case "pwd":        return CmdPwd();
            case "cd":         return CmdCd(args);
            case "cat":        return CmdCat(args);
            case "cp":         return CmdCp(args);
            case "mv":         return CmdMv(args);
            case "rm":         return CmdRm(args);
            case "nmap":       return CmdNmap(args);
            case "ping":       return CmdPing(args);
            case "hydra":      return CmdHydra(raw);
            case "ssh":        return CmdSsh(args);
            case "service":    return CmdService(args);
            case "whoami":     return CurrentUser;
            case "chmod":      return CmdChmod(args, isSudo);
            case "chown":      return CmdChown(args, isSudo);
            case "shutdown":   return CmdShutdown(args, isSudo);
            case "su":         return CmdSu(args);
            case "./hello.sh": return CmdRunScript(isSudo);
            default:           return Error($"-bash: {cmd}: command not found");
        }
    }

    string CmdLs(string[] args)
    {
        bool longFmt = false, showAll = false, reverse = false;
        string targetPath = CurrentDirectory;

        foreach (var a in args)
        {
            if (a.StartsWith("-"))
            {
                if (a.Contains("l")) longFmt = true;
                if (a.Contains("a")) showAll = true;
                if (a.Contains("r")) reverse = true;
            }
            else targetPath = ResolvePath(a);
        }

        if (!fileSystem.ContainsKey(targetPath))
            return Error($"ls: cannot access '{targetPath}': No such file or directory");

        var entries = new List<string>(fileSystem[targetPath]);
        if (showAll) entries.InsertRange(0, new[] { ".", ".." });
        if (reverse) entries.Reverse();

        var sb = new System.Text.StringBuilder();

        if (longFmt)
        {
            sb.AppendLine($"total {entries.Count * 4}");
            foreach (var e in entries)
            {
                if (e == "." || e == "..")
                { sb.AppendLine($"drwxr-xr-x  2 {CurrentUser} {CurrentUser}   4096 Dec 20  2017 {e}"); continue; }
                string fp    = CombinePath(targetPath, e);
                bool   isDir = fileSystem.ContainsKey(fp);
                string perm  = filePermissions.ContainsKey(fp) ? filePermissions[fp] : (isDir ? "drwxr-xr-x" : "-rw-r--r--");
                string owner = fileOwners.ContainsKey(fp) ? fileOwners[fp] : CurrentUser;
                string group = owner == "root" ? "root" : owner == "syslog" ? "adm" : owner == "utmp" ? "utmp" : owner;
                int    size  = isDir ? 4096 : (fileContents.ContainsKey(fp) ? System.Text.Encoding.UTF8.GetByteCount(fileContents[fp]) : 0);
                string links = isDir ? "2" : "1";
                string color = isDir ? "#6ad4ff" : "#ffffff";
                sb.AppendLine($"{perm}  {links} {owner} {group}  {size,6} Dec 20  2017 <color={color}>{e}</color>");
            }
        }
        else
        {
            // Calcular el ancho máximo para alinear en columnas
            int maxLen = 0;
            foreach (var e in entries)
                if (e.Length > maxLen) maxLen = e.Length;

            int colWidth  = maxLen + 2;   // padding entre columnas
            int termCols  = 4;            // columnas por fila (ajusta según tu terminal)
            int count     = 0;

            foreach (var e in entries)
            {
                string fp    = CombinePath(targetPath, e);
                bool   isDir = fileSystem.ContainsKey(fp) || e == "." || e == "..";
                string color = isDir ? "#6ad4ff" : "#ffffff";
                string label = isDir ? e + "/" : e;

                sb.Append($"<color={color}>{label}</color>");

                count++;
                // Salto de línea cada `termCols` entradas
                if (count % termCols == 0)
                    sb.AppendLine();
                else
                    sb.Append("  "); // separador entre columnas
            }

            // Si la última fila no llegó al límite, cerrar con newline
            if (count % termCols != 0)
                sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }

    string CmdPwd() => CurrentDirectory;

    string CmdCd(string[] args)
    {
        if (args.Length == 0 || args[0] == "~")
        { CurrentDirectory = IsRoot ? "/root" : "/home/sysadmin"; return ""; }

        string raw = args[0];
        if (raw.StartsWith("~/")) raw = (IsRoot ? "/root/" : "/home/sysadmin/") + raw.Substring(2);
        string target = ResolvePath(raw);

        if (!fileSystem.ContainsKey(target))
            return Error($"-bash: cd: {args[0]}: No such file or directory");

        CurrentDirectory = target;
        return "";
    }

    string CmdCat(string[] args)
    {
        if (args.Length == 0) return Error("cat: missing operand");
        string path = ResolvePath(args[0]);
        if (fileContents.ContainsKey(path)) return fileContents[path];
        if (fileSystem.ContainsKey(path))   return Error($"cat: {args[0]}: Is a directory");
        return Error($"cat: {args[0]}: No such file or directory");
    }

    string CmdCp(string[] args)
    {
        if (args.Length < 2) return Error("cp: missing file operand");
        string src  = ResolvePath(args[0]);
        string dest = ResolvePath(args[1]);
        if (fileSystem.ContainsKey(dest)) dest = CombinePath(dest, GetFileName(src));
        if (!fileContents.ContainsKey(src) && !fileSystem.ContainsKey(src))
            return Error($"cp: cannot stat '{args[0]}': No such file or directory");
        string dp = GetParent(dest);
        if (!fileSystem.ContainsKey(dp)) return Error($"cp: cannot create regular file '{args[1]}': No such file or directory");
        if (fileContents.ContainsKey(src)) fileContents[dest] = fileContents[src];
        fileOwners[dest] = CurrentUser; filePermissions[dest] = "-rw-r--r--";
        string dn = GetFileName(dest);
        if (!fileSystem[dp].Contains(dn)) fileSystem[dp].Add(dn);
        return "";
    }

    string CmdMv(string[] args)
    {
        if (args.Length < 2) return Error("mv: missing file operand");
        string dest = ResolvePath(args[args.Length - 1]);
        var errors  = new System.Text.StringBuilder();
        foreach (var srcArg in args.Take(args.Length - 1))
        {
            string src    = ResolvePath(srcArg);
            string target = fileSystem.ContainsKey(dest) ? CombinePath(dest, GetFileName(src)) : dest;
            string tp     = GetParent(target);
            if (!fileContents.ContainsKey(src) && !fileSystem.ContainsKey(src))
            { errors.AppendLine(Error($"mv: cannot stat '{srcArg}': No such file or directory")); continue; }
            if (!fileSystem.ContainsKey(tp))
            { errors.AppendLine(Error($"mv: cannot move '{srcArg}': No such file or directory")); continue; }
            if (fileContents.ContainsKey(src))    { fileContents[target]    = fileContents[src];    fileContents.Remove(src); }
            if (fileOwners.ContainsKey(src))      { fileOwners[target]      = fileOwners[src];      fileOwners.Remove(src); }
            if (filePermissions.ContainsKey(src)) { filePermissions[target] = filePermissions[src]; filePermissions.Remove(src); }
            string sp = GetParent(src), sn = GetFileName(src), tn = GetFileName(target);
            if (fileSystem.ContainsKey(sp)) fileSystem[sp].Remove(sn);
            if (!fileSystem[tp].Contains(tn)) fileSystem[tp].Add(tn);
        }
        return errors.ToString().TrimEnd();
    }

    string CmdRm(string[] args)
    {
        if (args.Length == 0) return Error("rm: missing operand");
        bool recursive = args.Any(a => a == "-r" || a == "-R" || a == "-rf" || a == "-fr");
        var  errors    = new System.Text.StringBuilder();
        foreach (var a in args)
        {
            if (a.StartsWith("-")) continue;
            string path   = ResolvePath(a);
            string parent = GetParent(path);
            string name   = GetFileName(path);
            if (fileSystem.ContainsKey(path))
            {
                if (!recursive) { errors.AppendLine(Error($"rm: cannot remove '{a}': Is a directory")); continue; }
                RemoveRecursive(path);
            }
            else if (fileSystem.ContainsKey(parent) && fileSystem[parent].Contains(name))
            { fileSystem[parent].Remove(name); fileContents.Remove(path); fileOwners.Remove(path); filePermissions.Remove(path); }
            else errors.AppendLine(Error($"rm: cannot remove '{a}': No such file or directory"));
        }
        return errors.ToString().TrimEnd();
    }

    string CmdChmod(string[] args, bool isSudo)
    {
        if (args.Length < 2) return Error("chmod: missing operand");
        string modeStr  = args[0];
        string fileName = args[args.Length - 1];
        string path     = ResolvePath(fileName);
        string owner    = fileOwners.ContainsKey(path) ? fileOwners[path] : "";
        if (!isSudo && owner != CurrentUser && CurrentUser != "root")
            return Error($"chmod: changing permissions of '{fileName}': Operation not permitted");
        if (!filePermissions.ContainsKey(path))
            return Error($"chmod: cannot access '{fileName}': No such file or directory");

        char[] perms  = filePermissions[path].ToCharArray();
        bool   add    = modeStr.Contains("+");
        bool   remove = modeStr.Contains("-") && modeStr.Length > 1;
        char   tgt    = modeStr[0];
        string pp     = modeStr.Length > 2 ? modeStr.Substring(2) : "";

        foreach (char p in pp)
        {
            int offset = p == 'r' ? 0 : p == 'w' ? 1 : p == 'x' ? 2 : -1;
            if (offset < 0) continue;
            if (tgt == 'u' || tgt == 'a') ApplyPerm(perms, 1 + offset, p, add, remove);
            if (tgt == 'g' || tgt == 'a') ApplyPerm(perms, 4 + offset, p, add, remove);
            if (tgt == 'o' || tgt == 'a') ApplyPerm(perms, 7 + offset, p, add, remove);
        }
        filePermissions[path] = new string(perms);
        return "";
    }

    void ApplyPerm(char[] perms, int idx, char p, bool add, bool remove)
    {
        if (idx >= perms.Length) return;
        if (add)    perms[idx] = p;
        if (remove) perms[idx] = '-';
    }

    string CmdChown(string[] args, bool isSudo)
    {
        if (!isSudo && CurrentUser != "root")
            return Error($"chown: changing ownership of '{(args.Length > 1 ? args[args.Length - 1] : "")}': Operation not permitted");
        if (args.Length < 2) return Error("chown: missing operand");
        string newOwner = args[0], fileName = args[args.Length - 1], path = ResolvePath(fileName);
        if (!fileOwners.ContainsKey(path) && !fileSystem.ContainsKey(path))
            return Error($"chown: cannot access '{fileName}': No such file or directory");
        fileOwners[path] = newOwner;
        return "";
    }

    string CmdShutdown(string[] args, bool isSudo)
    {
        if (!isSudo && CurrentUser != "root") return Error("shutdown: Need to be root");
        string time  = args.Length > 0 ? args[0] : "now";
        string extra = args.Length > 1 ? string.Join(" ", args.Skip(1)).Trim('"') : "";
        var    now   = DateTime.Now;
        string msg   = time == "now"
            ? "The system is going down for maintenance NOW!"
            : "The system is going down for maintenance in 1 minute!";
        string result = $"\nBroadcast message from {CurrentUser}@localhost\n        (/dev/pts/0) at {now.Hour}:{now.Minute:D2} ...\n\n{msg}";
        if (!string.IsNullOrEmpty(extra)) result += "\n" + extra;
        return result;
    }

    string CmdSu(string[] args)
    {
        bool switchRoot = args.Length == 0 || args[0] == "-" || args[0] == "root";
        if (!switchRoot) return "";
        IsRoot           = true;
        CurrentUser      = "root";
        CurrentDirectory = "/root";
        return "Password:";
    }


    string CmdRunScript(bool isSudo)
    {
        string path = CombinePath(CurrentDirectory, "hello.sh");
        if (!filePermissions.ContainsKey(path)) return Error("-bash: ./hello.sh: No such file or directory");
        string perm  = filePermissions[path];
        string owner = fileOwners.ContainsKey(path) ? fileOwners[path] : "";
        bool canExec = (isSudo || CurrentUser == "root")
                    || (CurrentUser == owner && perm.Length > 3 && perm[3] == 'x')
                    || (perm.Length > 9 && perm[9] == 'x');
        if (!canExec) return Error("-bash: ./hello.sh: Permission denied");
        return " ______________\n( Hello World! )\n --------------\n        \\\n         \\\n           <(^)\n            ( )";
    }

    string ResolvePath(string p)
    {
        if (p == "~")           return IsRoot ? "/root" : "/home/sysadmin";
        if (p == "..")          return GetParent(CurrentDirectory);
        if (p == ".")           return CurrentDirectory;
        if (p.StartsWith("~/")) return (IsRoot ? "/root/" : "/home/sysadmin/") + p.Substring(2);
        if (p.StartsWith("/"))  return Normalize(p);
        return Normalize(CurrentDirectory + "/" + p);
    }

    string Normalize(string path)
    {
        var stack = new List<string>();
        foreach (var part in path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (part == "..")     { if (stack.Count > 0) stack.RemoveAt(stack.Count - 1); }
            else if (part != ".") stack.Add(part);
        }
        return stack.Count == 0 ? "/" : "/" + string.Join("/", stack);
    }

    string GetParent(string path)
    {
        if (path == "/") return "/";
        int last = path.LastIndexOf('/');
        return last == 0 ? "/" : path.Substring(0, last);
    }

    string GetFileName(string path) => path.Contains("/") ? path.Substring(path.LastIndexOf('/') + 1) : path;
    string CombinePath(string a, string b) => a == "/" ? "/" + b : a + "/" + b;

    void RemoveRecursive(string path)
    {
        if (!fileSystem.ContainsKey(path)) return;
        foreach (var child in new List<string>(fileSystem[path]))
        {
            string cp = CombinePath(path, child);
            if (fileSystem.ContainsKey(cp)) RemoveRecursive(cp);
            else { fileContents.Remove(cp); fileOwners.Remove(cp); filePermissions.Remove(cp); }
        }
        fileSystem.Remove(path); fileContents.Remove(path);
        fileOwners.Remove(path); filePermissions.Remove(path);
        string parent = GetParent(path);
        if (fileSystem.ContainsKey(parent)) fileSystem[parent].Remove(GetFileName(path));
    }

    static string Error(string msg) => "ERROR:" + msg;

    string CmdMkdir(string[] args)
    {
        if (args.Length == 0) return Error("mkdir: missing operand");

        var sb = new System.Text.StringBuilder();
        foreach (var name in args)
        {
            string path = CombinePath(CurrentDirectory, name);

            if (fileSystem.ContainsKey(path) ||
                (fileSystem.ContainsKey(CurrentDirectory) &&
                fileSystem[CurrentDirectory].Contains(name)))
            {
                sb.AppendLine(Error($"mkdir: cannot create directory '{name}': File exists"));
                continue;
            }

            // Registrar en el sistema de archivos virtual
            if (fileSystem.ContainsKey(CurrentDirectory))
                fileSystem[CurrentDirectory].Add(name);

            fileSystem[path]      = new List<string>();
            fileOwners[path]      = CurrentUser;
            filePermissions[path] = "drwxr-xr-x";
        }
        return sb.ToString().TrimEnd();
    }

    string CmdTouch(string[] args)
    {
        if (args.Length == 0) return Error("touch: missing operand");

        foreach (var name in args)
        {
            string path = CombinePath(CurrentDirectory, name);

            // Si ya existe (archivo o directorio), touch no hace nada
            if (fileContents.ContainsKey(path) || fileSystem.ContainsKey(path))
                continue;

            // Crear archivo vacío
            if (fileSystem.ContainsKey(CurrentDirectory))
                fileSystem[CurrentDirectory].Add(name);

            fileContents[path]    = "";
            fileOwners[path]      = CurrentUser;
            filePermissions[path] = "-rw-r--r--";
        }
        return "";
    }
    string CmdEcho(string raw)
    {
        // Detectar redirección >> (append) o > (sobreescribir)
        bool isAppend   = raw.Contains(">>");
        bool isRedirect = raw.Contains(">");

        if (!isRedirect)
        {
            // echo sin redirección: imprime el texto después de "echo"
            string text = raw.Length > 4 ? raw.Substring(4).Trim() : "";
            text = text.Trim('"').Trim('\'');
            return text;
        }

        // Separar en: [parte izquierda] >> o > [nombre de archivo]
        string sep      = isAppend ? ">>" : ">";
        int    sepIdx   = raw.IndexOf(sep);
        string leftPart = raw.Substring(0, sepIdx).Trim();
        string fileName = raw.Substring(sepIdx + sep.Length).Trim().Trim('"').Trim('\'');

        if (string.IsNullOrEmpty(fileName))
        return Error("bash: syntax error near unexpected token 'newline'");

        // Extraer el texto (quitar la palabra "echo" y las comillas)
        string content = leftPart.Length > 4 ? leftPart.Substring(4).Trim() : "";
        content = content.Trim('"').Trim('\'');

        string filePath = CombinePath(CurrentDirectory, fileName);

        if (fileSystem.ContainsKey(filePath))
            return Error($"bash: {fileName}: Is a directory");

        if (fileContents.ContainsKey(filePath))
        {
            // El archivo ya existe: sobreescribir o agregar
            fileContents[filePath] = isAppend
                ? fileContents[filePath] + "\n" + content
                : content;
        }
        else
        {
            // Crear el archivo con el contenido
            if (fileSystem.ContainsKey(CurrentDirectory))
                fileSystem[CurrentDirectory].Add(fileName);

            fileContents[filePath]    = content;
            fileOwners[filePath]      = CurrentUser;
            filePermissions[filePath] = "-rw-r--r--";
        }

        return ""; 
    }

    string CmdIfconfig(string[] args)
    {
    // Sin argumentos: muestra todas las interfaces
    if (args.Length == 0)
    {
        return
            "eth0: flags=4163<UP,BROADCAST,RUNNING,MULTICAST>  mtu 1500\n" +
            "        inet 192.168.1.105  netmask 255.255.255.0  broadcast 192.168.1.255\n" +
            "        inet6 fe80::a00:27ff:fe4e:66a1  prefixlen 64  scopeid 0x20<link>\n" +
            "        ether 08:00:27:4e:66:a1  txqueuelen 1000  (Ethernet)\n" +
            "        RX packets 8523  bytes 9216384 (8.7 MiB)\n" +
            "        RX errors 0  dropped 0  overruns 0  frame 0\n" +
            "        TX packets 4271  bytes 614123 (599.7 KiB)\n" +
            "        TX errors 0  dropped 0 overruns 0  carrier 0  collisions 0\n" +
            "\n" +
            "lo: flags=73<UP,LOOPBACK,RUNNING>  mtu 65536\n" +
            "        inet 127.0.0.1  netmask 255.0.0.0\n" +
            "        inet6 ::1  prefixlen 128  scopeid 0x10<host>\n" +
            "        loop  txqueuelen 1000  (Local Loopback)\n" +
            "        RX packets 120  bytes 10440 (10.1 KiB)\n" +
            "        TX packets 120  bytes 10440 (10.1 KiB)";
    }

    // ifconfig eth0 o ifconfig lo
    string iface = args[0];
    if (iface == "eth0")
    {
        return
            "eth0: flags=4163<UP,BROADCAST,RUNNING,MULTICAST>  mtu 1500\n" +
            "        inet 192.168.1.105  netmask 255.255.255.0  broadcast 192.168.1.255\n" +
            "        inet6 fe80::a00:27ff:fe4e:66a1  prefixlen 64  scopeid 0x20<link>\n" +
            "        ether 08:00:27:4e:66:a1  txqueuelen 1000  (Ethernet)\n" +
            "        RX packets 8523  bytes 9216384 (8.7 MiB)\n" +
            "        TX packets 4271  bytes 614123 (599.7 KiB)";
    }
    if (iface == "lo")
    {
        return
            "lo: flags=73<UP,LOOPBACK,RUNNING>  mtu 65536\n" +
            "        inet 127.0.0.1  netmask 255.0.0.0\n" +
            "        inet6 ::1  prefixlen 128  scopeid 0x10<host>\n" +
            "        loop  txqueuelen 1000  (Local Loopback)";
    }

    return Error($"ifconfig: interface '{iface}' does not exist");
    }

    bool   m2_objetivoPingado    = false;
    bool   m2_puertoEscaneado    = false;
    bool   m2_wordlistVista      = false;
    bool   m2_ataqueRealizado    = false;
    bool   m2_sesionSSH          = false;
    string m2_passwordEncontrada = "batman123"; 
    string m2_objetivoIP         = "192.168.1.200";
    bool   esperandoPasswordSSH = false;
    string sshUserPendiente     = "";
    string sshHostPendiente     = "";

    string CmdNmap(string[] args)
    {
        if (args.Length == 0)
            return Error("nmap: you must specify a target");

        // Filtrar flags para obtener la IP
        string target = "";
        bool   sV     = false;
        bool   sS     = false;

        foreach (var a in args)
        {
            if (a == "-sV") sV = true;
            else if (a == "-sS" || a == "-sn") sS = true;
            else if (!a.StartsWith("-")) target = a;
        }

        if (target != m2_objetivoIP)
            return
                $"Starting Nmap 7.92 ( https://nmap.org )\n" +
                $"Nmap scan report for {target}\n" +
                $"Host seems down. If it is really up, but blocking our ping probes, try -Pn\n" +
                $"Nmap done: 1 IP address (0 hosts up) scanned in 3.12 seconds";

        m2_puertoEscaneado = true;

        if (sV)
        {
            return
                $"Starting Nmap 7.92 ( https://nmap.org )\n" +
                $"Nmap scan report for {m2_objetivoIP}\n" +
                $"Host is up (0.00091s latency).\n" +
                $"\n" +
                $"PORT    STATE  SERVICE  VERSION\n" +
                $"22/tcp  open   ssh      OpenSSH 7.4 (protocol 2.0)\n" +
                $"80/tcp  open   http     Apache httpd 2.4.6\n" +
                $"443/tcp closed https\n" +
                $"\n" +
                $"Service detection performed.\n" +
                $"Nmap done: 1 IP address (1 host up) scanned in 8.43 seconds";
        }

        return
            $"Starting Nmap 7.92 ( https://nmap.org )\n" +
            $"Nmap scan report for {m2_objetivoIP}\n" +
            $"Host is up (0.00091s latency).\n" +
            $"\n" +
            $"PORT    STATE  SERVICE\n" +
            $"22/tcp  open   ssh\n" +
            $"80/tcp  open   http\n" +
            $"443/tcp closed https\n" +
            $"\n" +
            $"Nmap done: 1 IP address (1 host up) scanned in 5.21 seconds";
    }

    //  HYDRA 
    string CmdHydra(string raw)
    {
        // Validar que tenga los parámetros mínimos
        bool tieneL  = raw.Contains("-l ");
        bool tieneP  = raw.Contains("-P ");
        bool tieneSsh = raw.Contains("ssh://");

        if (!tieneL || !tieneP || !tieneSsh)
        {
            return
                "Hydra v9.3 (c) 2022 by van Hauser/THC\n" +
                "Syntax: hydra -l user -P passlist.txt ssh://target\n" +
                "        hydra -L userlist.txt -P passlist.txt ssh://target\n" +
                Error("hydra: missing required parameters (-l, -P, target)");
        }

        if (!m2_puertoEscaneado)
            return
                "[WARNING] Hydra: target 192.168.1.200 — connection refused\n" +
                "[ERROR] could not connect to ssh://192.168.1.200:22\n" +
                "Tip: scan the target first with nmap to verify port 22 is open";

        // Simular el ataque con progreso
        m2_ataqueRealizado = true;

        return
            "Hydra v9.3 (c) 2022 by van Hauser/THC & David Maciejak\n" +
            "Hydra (https://github.com/vanhauser-thc/thc-hydra)\n" +
            $"[DATA] max 16 tasks per 1 server, overall 16 tasks\n" +
            $"[DATA] attacking ssh://{m2_objetivoIP}:22/\n" +
            "[STATUS] 16.00 tries/min, 16 tries in 00:01h\n" +
            "[STATUS] 112.00 tries/min, 112 tries in 00:02h\n" +
            "[STATUS] Trying admin:123456 ...\n" +
            "[STATUS] Trying admin:qwerty ...\n" +
            "[STATUS] Trying admin:letmein ...\n" +
            "[STATUS] Trying admin:password123 ...\n" +
            $"[22][ssh] host: {m2_objetivoIP}   login: admin   password: {m2_passwordEncontrada}\n" +
            "1 of 1 target successfully completed, 1 valid password found\n" +
            "Hydra (https://github.com/vanhauser-thc/thc-hydra) finished.";
    }

    //  SSH 
    string CmdSsh(string[] args)
    {
        if (args.Length == 0)
            return Error("ssh: missing target. Usage: ssh user@host");

        string target = args[args.Length - 1];

        if (!target.Contains("@"))
            return Error("ssh: invalid format. Use: ssh user@host");

        string[] parts = target.Split('@');
        string   user  = parts[0];
        string   host  = parts[1];

        if (host != m2_objetivoIP)
            return $"ssh: connect to host {host} port 22: Connection refused";

        if (!m2_ataqueRealizado)
            return
                $"{user}@{host}'s password:\n" +
                $"Permission denied, please try again.\n" +
                $"Permission denied (publickey,password).";

        // Activar estado de espera, el próximo input será la contraseña
        esperandoPasswordSSH = true;
        sshUserPendiente     = user;
        sshHostPendiente     = host;

        return
            $"The authenticity of host '{host}' can't be established.\n" +
            $"ECDSA key fingerprint is SHA256:xKd9eFm3vQpL8nRt2jYs6wCb1oMu4pHe.\n" +
            $"Warning: Permanently added '{host}' (ECDSA) to the list of known hosts.\n" +
            $"{user}@{host}'s password:";  // ← aquí el estudiante escribe la contraseña
    }

    string ProcesarPasswordSSH(string input)
    {
        esperandoPasswordSSH = false; // reset del estado siempre

        if (input == m2_passwordEncontrada)
        {
            // Contraseña correcta — iniciar sesión
            m2_sesionSSH     = true;
            CurrentUser      = sshUserPendiente;
            CurrentDirectory = $"/home/{sshUserPendiente}";

            InicializarServidorObjetivo();

            return
                $"Linux target-server 4.19.0-kali3-amd64 #1 SMP Kali 4.19.20\n" +
                $"Last login: Mon Jan 15 09:23:11 2024 from 192.168.1.50\n" +
                $"Welcome to target-server!\n" +
                $"{sshUserPendiente}@target-server:~$";
        }
        else
        {
            // Contraseña incorrecta — dar una segunda oportunidad
            esperandoPasswordSSH = true; // volver a esperar
            intentosSSH++;

            if (intentosSSH >= 3)
            {
                // Demasiados intentos
                esperandoPasswordSSH = false;
                intentosSSH          = 0;
                return
                    $"{sshUserPendiente}@{sshHostPendiente}'s password:\n" +
                    $"Permission denied, please try again.\n" +
                    $"{sshUserPendiente}@{sshHostPendiente}'s password:\n" +
                    $"Permission denied (publickey,password).\n" +
                    $"Tip: usa el comando que encontraste con hydra";
            }

            return $"Permission denied, please try again.\n{sshUserPendiente}@{sshHostPendiente}'s password:";
        }
    }
    int intentosSSH = 0;

    //Inicializar filesystem del servidor objetivo
    void InicializarServidorObjetivo()
    {
        string home = $"/home/admin";

        // Evitar duplicar si ya existe
        if (fileSystem.ContainsKey(home)) return;

        // Estructura del servidor objetivo
        fileSystem[home]              = new List<string> { "notas.txt", "backup" };
        fileSystem[$"{home}/backup"]  = new List<string> { "db_backup.sql", "config.bak" };
        fileSystem["/root"]           = new List<string> { "secreto.txt", "flag.txt" };
        fileSystem["/root"]           = new List<string> { "secreto.txt", "flag.txt" };

        fileContents[$"{home}/notas.txt"] =
            "Recordatorio: cambiar contraseña del servidor\n" +
            "IP base de datos: 10.0.0.5\n" +
            "Usuario DB: dbadmin\n" +
            "IMPORTANTE: revisa la carpeta backup, hay credenciales viejas\n" +
            "El administrador root guarda cosas importantes en su carpeta";

        fileContents[$"{home}/backup/db_backup.sql"] =
            "-- MySQL dump\n" +
            "-- Host: localhost\n" +
            "CREATE TABLE users (\n" +
            "  id INT PRIMARY KEY,\n" +
            "  username VARCHAR(50),\n" +
            "  password VARCHAR(255)\n" +
            ");\n" +
            "INSERT INTO users VALUES (1,'admin','$2y$10$hashed_password');";

        fileContents[$"{home}/backup/config.bak"] =
            "DB_HOST=10.0.0.5\n" +
            "DB_USER=dbadmin\n" +
            "DB_PASS=Sup3rS3cr3t!\n" +
            "APP_KEY=base64:kDx92mNvPqL3rTsW\n" +
            "# NOTA: el archivo de configuracion maestro esta en /root/secreto.txt\n" +
            "# Solo root puede leerlo";

        fileContents["/root/secreto.txt"] =
            "=== ARCHIVO CONFIDENCIAL ===\n" +
            "Credenciales sistema de nomina:\n" +
            "Usuario: finanzas_admin\n" +
            "Password: N0m1n4_2024!\n" +
            "Servidor: 10.0.0.10:3306\n" +
            "---\n" +
            "Ver flag.txt para confirmacion de acceso";

        fileContents["/root/flag.txt"] =
            "  ███████╗██╗      █████╗  ██████╗ \n" +
            "  ██╔════╝██║     ██╔══██╗██╔════╝ \n" +
            "  █████╗  ██║     ███████║██║  ███╗\n" +
            "  ██╔══╝  ██║     ██╔══██║██║   ██║\n" +
            "  ██║     ███████╗██║  ██║╚██████╔╝\n" +
            "  ╚═╝     ╚══════╝╚═╝  ╚═╝ ╚═════╝ \n" +
            "\n" +
            "FLAG{br4ck3t_h4ck3r_m0dul0_2_c0mpl3t4d0}\n" +
            "\n" +
            "¡Felicitaciones! Completaste el Modulo 2";

        fileOwners[$"{home}/notas.txt"]          = "admin";
        fileOwners[$"{home}/backup/db_backup.sql"] = "admin";
        fileOwners[$"{home}/backup/config.bak"]  = "admin";
        fileOwners["/root/secreto.txt"]          = "root";
        fileOwners["/root/flag.txt"]             = "root";
        filePermissions[$"{home}/notas.txt"]     = "-rw-r--r--";
        filePermissions["/root/secreto.txt"]     = "-rw-------";
        filePermissions["/root/flag.txt"]        = "-rw-------";
    }

    // SERVICE (iniciar/detener servicios)
    string CmdService(string[] args)
    {
        if (args.Length < 2)
            return Error("service: Usage: service <name> <start|stop|status>");

        string name   = args[0];
        string action = args[1];

        if (action == "status")
        {
            bool activo = name == "ssh" || name == "apache2";
            return activo
                ? $"● {name}.service - {name} server\n" +
                  $"   Loaded: loaded (/lib/systemd/system/{name}.service)\n" +
                  $"   Active: active (running) since Mon 2024-01-15 09:00:00 UTC"
                : $"● {name}.service\n" +
                  $"   Loaded: loaded (/lib/systemd/system/{name}.service)\n" +
                  $"   Active: inactive (dead)";
        }

        return $"[ ok ] {action}ing {name} daemon...";
    }

    string CmdPing(string[] args)
    {
        if (args.Length == 0) 
            return Error("ping: usage error: Destination address required");

        int    count = 4;
        string host  = "";

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-c" && i + 1 < args.Length)
                int.TryParse(args[++i], out count);
            else if (!args[i].StartsWith("-"))
                host = args[i];
        }

        if (string.IsNullOrEmpty(host))
            return Error("ping: usage error: Destination address required");

        // IP simulada según host
        string ip = host == "localhost" || host == "127.0.0.1"
            ? "127.0.0.1"
            : host == m2_objetivoIP
                ? m2_objetivoIP
                : host.Replace("www.", "").Length > 0
                    ? "93.184.216.34"
                    : host;

        // Si es el objetivo del módulo 2, marcar como pingado
        if (host == m2_objetivoIP)
            m2_objetivoPingado = true;

        count = Mathf.Clamp(count, 1, 6);

        var    sb  = new System.Text.StringBuilder();
        var    rng = new System.Random();

        sb.AppendLine($"PING {host} ({ip}) 56(84) bytes of data.");

        for (int i = 0; i < count; i++)
        {
            float ms = (float)(rng.NextDouble() * 20 + 1);
            sb.AppendLine($"64 bytes from {ip}: icmp_seq={i + 1} ttl=64 time={ms:F2} ms");
        }

        sb.AppendLine($"");
        sb.AppendLine($"--- {host} ping statistics ---");
        sb.AppendLine($"{count} packets transmitted, {count} received, 0% packet loss");
        sb.Append($"rtt min/avg/max = 1.0/10.5/21.0 ms");

        return sb.ToString();
    }

    public List<string> GetEntradas(string dirPath)
    {
        if (!fileSystem.ContainsKey(dirPath)) return new List<string>();
        return new List<string>(fileSystem[dirPath]);
    }
}