Redis(TM) can be accessed on the following DNS names from within your cluster:

`signaller-cache-redis-master.signaller.svc.cluster.local` for read/write operations (port 6379)
`signaller-cache-redis-replicas.signaller.svc.cluster.local` for read-only operations (port 6379)

To get your password run:

```bash
export REDIS_PASSWORD=$(kubectl get secret --namespace signaller signaller-cache-redis -o jsonpath="{.data.redis-password}" | base64 --decode)
```

To connect to your Redis(TM) server:

First, run a Redis(TM) pod that you can use as a client:

```bash
kubectl run --namespace signaller redis-client --restart='Never' --env REDIS_PASSWORD=$REDIS_PASSWORD --image docker.io/bitnami/redis:6.2.4-debian-10-r13 --command -- sleep infinity
```

Use the following command to attach to the pod:

```bash
kubectl exec --tty -i redis-client --namespace signaller -- bash
```

Then, connect using the Redis(TM) CLI:

```bash
redis-cli -h signaller-cache-redis-master -a $REDIS_PASSWORD
redis-cli -h signaller-cache-redis-replicas -a $REDIS_PASSWORD
```

To connect to your database from outside the cluster execute the following commands:

```bash
kubectl port-forward --namespace signaller svc/signaller-cache-redis-master 6379:6379 &
redis-cli -h 127.0.0.1 -p 6379 -a $REDIS_PASSWORD
```
