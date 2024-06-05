Install using;

```bash
helm upgrade --install anz abpzerotemplate-mvc
```

Uninstall all charts

```bash
helm uninstall anz
```

## Create Images

```bash
docker build -t abpzerotemplate-mvc -f src\prod.Web.Mvc\Dockerfile .
docker build -t abpzerotemplate-migrator -f src\prod.Migrator\Dockerfile .
```
