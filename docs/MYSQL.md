> Tip:
> 
> Watch the deployment status using the command: kubectl get pods -w --namespace signaller

Services:

```bash
echo Primary: signaller-db-mysql.signaller.svc.cluster.local:3306
```

Administrator credentials:

```bash
echo Username: root
echo Password: $(kubectl get secret --namespace signaller signaller-db-mysql -o jsonpath="{.data.mysql-root-password}" | base64 --decode)
```

To connect to your database:

First, run a pod that you can use as a client:

```bash
kubectl run signaller-db-mysql-client --rm --tty -i --restart='Never' --image  docker.io/bitnami/mysql:8.0.25-debian-10-r37 --namespace signaller --command -- bash
```

Then, connect to primary service (read/write):

```bash
mysql -h signaller-db-mysql.signaller.svc.cluster.local -uroot -p signaller
```

To upgrade this helm chart:

Obtain the password as described on the 'Administrator credentials' section and set the 'root.password' parameter as shown below:

```bash
ROOT_PASSWORD=$(kubectl get secret --namespace signaller signaller-db-mysql -o jsonpath="{.data.mysql-root-password}" | base64 --decode)

helm upgrade --namespace signaller signaller-db bitnami/mysql --set auth.rootPassword=$ROOT_PASSWORD
```
