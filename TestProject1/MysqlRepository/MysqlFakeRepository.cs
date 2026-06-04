using AvaloniaMySql1125Repository.DB;

namespace TestProject1;

public class FakeTable
{
}

public class MysqlFakeRepository : MySqlRepository<FakeTable>
{
    public MysqlFakeRepository(MySqlConnect connector) : base(connector)
    {
    }

    public override FakeTable GetById(int id)
    {
        throw new NotImplementedException();
    }

    public override List<FakeTable> GetAll()
    {
        throw new NotImplementedException();
    }

    public override bool Insert(FakeTable entity)
    {
        throw new NotImplementedException();
    }

    public override bool Update(FakeTable entity)
    {
        throw new NotImplementedException();
    }

    public override bool Delete(FakeTable entity)
    {
        throw new NotImplementedException();
    }
}